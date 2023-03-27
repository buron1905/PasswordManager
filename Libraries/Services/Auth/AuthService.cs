using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using Services.Cryptography;
using Services.TMP;
using System.Security.Claims;
using System.Text;

namespace Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IDataServiceWrapper _dataServiceWrapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly AppSettings? _appSettings;

        public AuthService(IDataServiceWrapper dataServiceWrapper, IRepositoryWrapper repositoryWrapper, ITwoFactorAuthService twoFactorAuthService, IJwtService jwtService, IEmailService emailService, IOptions<AppSettings>? appSettings = null)
        {
            _dataServiceWrapper = dataServiceWrapper;
            _repositoryWrapper = repositoryWrapper;
            _twoFactorAuthService = twoFactorAuthService;
            _jwtService = jwtService;
            _emailService = emailService;
            _appSettings = appSettings?.Value;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (!await AuthUser(requestDTO.EmailAddress!, requestDTO.Password!))
                return null;

            //get user guid
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.EmailAddress!);
            var response = GetAuthResponse(userDTO.Id, requestDTO.EmailAddress!, requestDTO.Password!, true, userDTO.TwoFactorEnabled, !userDTO.TwoFactorEnabled, emailVerified: userDTO.EmailConfirmed);

            return response;
        }

        public async Task<AuthResponseDTO?> LoginTfaAsync(LoginTfaRequestDTO requestDTO)
        {
            if (requestDTO is null)
                return null;
            if (requestDTO.Token is null || requestDTO.Code is null)
                return null;

            var claims = _jwtService.ValidateJweToken(requestDTO.Token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);
            if (claims is null)
                return null;

            var userId = JwtService.GetUserGuidFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);
            var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            if (userDTO is null) return null;

            var secret = await DecryptString(password, userDTO.TwoFactorSecret);

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(secret, requestDTO.Code!);

            if (!valid)
                return null;

            var emailAddress = JwtService.GetUserEmailFromClaims(claims);

            var response = GetAuthResponse(userId, emailAddress, password, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }

        public bool ValidateTfaCode(string secret, string code)
        {
            return _twoFactorAuthService.ValidateTwoFactorPin(secret, code);
        }

        public async Task<UserDTO?> SetTwoFactorEnabledAsync(Guid userId, string password)
        {
            var user = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            user.TwoFactorEnabled = true;
            return await _dataServiceWrapper.UserService.UpdateAsync(userId, user);
        }

        public async Task<UserDTO?> SetTwoFactorDisabledAsync(Guid userId)
        {
            var user = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            user.TwoFactorEnabled = false;
            return await _dataServiceWrapper.UserService.UpdateAsync(userId, user);
        }

        public TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            return _twoFactorAuthService.GenerateTfaSetup(issuer, accountTitle, accountSecretKey);
        }

        public async Task<string> DecryptString(string password, string textEncrypted)
        {
            var textDecrypted = await EncryptionService.DecryptAsync(Encoding.Unicode.GetBytes(textEncrypted),
                password);

            return textDecrypted;
        }

        public async Task<TfaSetupDTO?> EnableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            var secret = await DecryptString(password, userDTO.TwoFactorSecret);

            var isValidCode = ValidateTfaCode(secret, tfaSetupDTO.Code);

            if (!isValidCode)
                return null;

            userDTO = await SetTwoFactorEnabledAsync(userId, password);

            var tfaResponse = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, secret!);
            tfaResponse.IsTfaEnabled = true;

            return tfaResponse;
        }


        public async Task<TfaSetupDTO?> DisableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            var secret = await DecryptString(password, userDTO.TwoFactorSecret);

            var isValidCode = ValidateTfaCode(secret, tfaSetupDTO.Code);

            if (!isValidCode)
                return null;

            userDTO = await SetTwoFactorDisabledAsync(userId);

            var tfaResponse = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, secret!);
            tfaResponse.IsTfaEnabled = false;

            return tfaResponse;
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (await _repositoryWrapper.UserRepository.AnyAsync(user => user.EmailAddress.Equals(requestDTO.EmailAddress!)))
                throw new AppException("Email is already used by another user.");

            requestDTO.PasswordHASH = HashingService.HashPassword(requestDTO.Password);

            var userDTO = await _dataServiceWrapper.UserService.CreateAsync(requestDTO);

            // Temporarily turned off for MAUI testing
            //_emailService.SendRegistrationEmail(userDTO.EmailAddress, userDTO.EmailConfirmationToken);

            return GetAuthResponse(userDTO.Id, userDTO.EmailAddress!, requestDTO.Password!, emailVerified: userDTO.EmailConfirmed);
        }

        public AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            var expires = DateTime.UtcNow.AddMinutes(_appSettings?.JweTokenMinutesTTL ?? 5);
            var claims = _jwtService.GetClaims(userId, emailAddress, password, expires, tfaChecked, emailVerified);
            var tokenString = _jwtService.GenerateJweToken(claims, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey, expires);

            return new AuthResponseDTO
            {
                IsAuthSuccessful = isAuthSuccessful,
                EmailVerified = emailVerified,
                IsTfaEnabled = tfaEnabled,
                JweToken = tokenString,
                ExpirationDateTime = expires
            };
        }

        public async Task<AuthResponseDTO?> RefreshTokenAsync(string token)
        {
            IEnumerable<Claim>? claims = null;
            claims = _jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);
            if (claims is null)
                return null;

            var userId = claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            var emailAddress = claims.First(c => c.Type.Equals(ClaimTypes.Email))?.Value;
            var password = claims.FirstOrDefault(claim => claim.Type.Equals("password"))?.Value;
            var tfaChecked = claims.FirstOrDefault(claim => claim.Type.Equals("tfaChecked"))?.Value == true.ToString();
            var emailConfirmed = claims.FirstOrDefault(claim => claim.Type.Equals("emailConfirmed"))?.Value == true.ToString();

            if (!await AuthUser(emailAddress!, password!))
                return null;

            var response = GetAuthResponse(new Guid(userId!), emailAddress!, password!, tfaChecked: tfaChecked, emailVerified: emailConfirmed);

            return response;
        }

        public bool TokenIsValid(string token)
        {
            var password = _jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);

            return password != null;
        }

        // helper methods

        private bool IsRequestValid(LoginRequestDTO requestDTO)
        {
            if (requestDTO is null)
                return false;
            return requestDTO.EmailAddress is not null || requestDTO.Password is not null;
        }

        private async Task<bool> AuthUser(string emailAddress, string password)
        {
            var user = await _dataServiceWrapper.UserService.GetByEmailAsync(emailAddress);
            if (user is null)
                return false;
            //throw new UserNotFoundException(emailAddress);

            if (!HashingService.Verify(password, user.PasswordHASH))
                return false;
            //throw new PasswordNotFoundException();

            return true;
        }

        public async Task<TfaSetupDTO?> GetTfaSetup(Guid userId, string password)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            if (userDTO.TwoFactorSecret is null)
            {
                var newTwoFactorSecret = Guid.NewGuid().ToString().Trim().Replace("-", "").Substring(0, 10);
                newTwoFactorSecret = Encoding.Unicode.GetString(await EncryptionService.EncryptAsync(newTwoFactorSecret,
                    password));
                userDTO.TwoFactorSecret = newTwoFactorSecret;
                userDTO = await _dataServiceWrapper.UserService.UpdateAsync(userId, userDTO);
            }

            var secret = await DecryptString(password, userDTO.TwoFactorSecret!);

            var result = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, secret!);
            result.IsTfaEnabled = userDTO.TwoFactorEnabled;

            return result;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(email);

            if (userDTO is null) return false;
            if (userDTO.EmailConfirmed)
                return true;
            if (userDTO.EmailConfirmationToken is null) return false;

            if (userDTO.EmailConfirmationToken.Equals(token))
            {
                userDTO.EmailConfirmed = true;
                await _dataServiceWrapper.UserService.UpdateAsync(userDTO.Id, userDTO);
                return true;
            }
            return false;
        }

        public async Task ResendConfirmEmail(string email)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(email);

            _emailService.SendRegistrationEmail(email, userDTO.EmailConfirmationToken);
        }

        public async Task<AuthResponseDTO?> LoginTfaAsync(string code, string email, string password)
        {
            if (code is null || email is null || password is null)
                return null;

            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(email);
            if (userDTO is null) return null;

            var secret = await DecryptString(password, userDTO.TwoFactorSecret);

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(secret, code!);

            if (!valid)
                return null;

            var response = GetAuthResponse(userDTO.Id, email, password, true, true, true, userDTO.EmailConfirmed);

            return response;
        }
    }
}
