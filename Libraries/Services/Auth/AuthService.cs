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

            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.EmailAddress!);

            var response = GetAuthResponse(userDTO, requestDTO.EmailAddress!, requestDTO.Password!, true, userDTO.TwoFactorEnabled, !userDTO.TwoFactorEnabled, emailVerified: userDTO.EmailConfirmed);

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

            var response = GetAuthResponse(userDTO, emailAddress, password, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

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
            return await _dataServiceWrapper.UserService.UpdateAsync(user);
        }

        public async Task<UserDTO?> SetTwoFactorDisabledAsync(Guid userId)
        {
            var user = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            user.TwoFactorEnabled = false;
            return await _dataServiceWrapper.UserService.UpdateAsync(user);
        }

        public TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            return _twoFactorAuthService.GenerateTfaSetup(issuer, accountTitle, accountSecretKey);
        }

        public async Task<string> DecryptString(string password, string textEncrypted)
        {
            var textDecrypted = await EncryptionService.DecryptAsync(Convert.FromBase64String(textEncrypted),
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

        public async Task<RegisterResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (await _repositoryWrapper.UserRepository.AnyAsync(user => user.EmailAddress.Equals(requestDTO.EmailAddress!)))
                throw new AppException("Email is already used by another user.");

            var userDTO = new UserDTO()
            {
                EmailAddress = requestDTO.EmailAddress,
                PasswordHASH = HashingService.HashPassword(requestDTO.Password!),
                TwoFactorEnabled = false,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                TwoFactorSecret = Convert.ToBase64String(await EncryptionService.EncryptAsync(Guid.NewGuid().ToString().Trim().Replace("-", "").Substring(0, 10), requestDTO.Password!))
            };

            userDTO = await _dataServiceWrapper.UserService.CreateAsync(userDTO);

            _emailService.SendRegistrationEmail(userDTO.EmailAddress, userDTO.EmailConfirmationToken);

            userDTO.Password = string.Empty;

            return new RegisterResponseDTO() { User = userDTO, IsRegistrationSuccessful = true };
        }

        public AuthResponseDTO GetAuthResponse(UserDTO user, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            var expires = DateTime.UtcNow.AddMinutes(_appSettings?.JweTokenMinutesTTL ?? 5);
            var claims = _jwtService.GetClaims(user.Id, emailAddress, password, expires, tfaChecked, emailVerified);
            var tokenString = _jwtService.GenerateJweToken(claims, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey, expires);

            return new AuthResponseDTO
            {
                User = user,
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

            var user = await _dataServiceWrapper.UserService.GetByIdAsync(new Guid(userId!));

            var response = GetAuthResponse(user, emailAddress!, password!, tfaChecked: tfaChecked, emailVerified: emailConfirmed);

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
            return requestDTO.EmailAddress is not null && requestDTO.Password is not null;
        }

        private async Task<bool> AuthUser(string emailAddress, string password)
        {
            var user = await _dataServiceWrapper.UserService.GetByEmailAsync(emailAddress);
            if (user is null)
                return false;
            //throw new UserNotFoundException(emailAddress);

            if (HashingService.Verify(password, user.PasswordHASH))
                return true;
            //throw new PasswordNotFoundException();

            return false;
        }

        public async Task<TfaSetupDTO?> GetTfaSetup(Guid userId, string password)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            if (userDTO.TwoFactorSecret is null)
            {
                var newTwoFactorSecret = Guid.NewGuid().ToString().Trim().Replace("-", "").Substring(0, 10);
                newTwoFactorSecret = Convert.ToBase64String(await EncryptionService.EncryptAsync(newTwoFactorSecret,
                    password));
                userDTO.TwoFactorSecret = newTwoFactorSecret;
                userDTO = await _dataServiceWrapper.UserService.UpdateAsync(userDTO);
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
            if (userDTO.EmailConfirmationToken is null) return true;

            if (userDTO.EmailConfirmationToken.Equals(token))
            {
                userDTO.EmailConfirmed = true;
                await _dataServiceWrapper.UserService.UpdateAsync(userDTO);
                return true;
            }
            return false;
        }

        public async Task<bool> ResendConfirmEmail(string email)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(email);

            if (userDTO is null) return false;

            _emailService.SendRegistrationEmail(email, userDTO.EmailConfirmationToken!);
            return true;
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

            var response = GetAuthResponse(userDTO, email, password, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }

        public async Task<AuthResponseDTO?> LoginWithTfaAsync(LoginWithTfaRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (!await AuthUser(requestDTO.EmailAddress!, requestDTO.Password!))
                return null;

            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.EmailAddress);
            if (userDTO is null) return null;

            var secret = await DecryptString(requestDTO.Password, userDTO.TwoFactorSecret);

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(secret, requestDTO.Code!);

            if (!valid)
                return null;

            var response = GetAuthResponse(userDTO, userDTO.EmailAddress, requestDTO.Password, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }
    }
}
