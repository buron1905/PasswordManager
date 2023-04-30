using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Exceptions;
using Services.Cryptography;
using Services.TMP;
using System.Security.Claims;

namespace Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly AppSettings? _appSettings;

        public AuthService(IUserService userService, ITwoFactorAuthService twoFactorAuthService, IJwtService jwtService, IEmailService emailService, IOptions<AppSettings>? appSettings = null)
        {
            _userService = userService;
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

            var userDTO = await _userService.GetByEmailAsync(requestDTO.EmailAddress!);

            var response = GetAuthResponse(userDTO, requestDTO.EmailAddress!, true, userDTO.TwoFactorEnabled, !userDTO.TwoFactorEnabled, userDTO.EmailConfirmed);

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
            var userDTO = await _userService.GetByIdAsync(userId);
            if (userDTO is null) return null;

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(userDTO.TwoFactorSecret, requestDTO.Code!);

            if (!valid)
                return null;

            var emailAddress = JwtService.GetUserEmailFromClaims(claims);

            var response = GetAuthResponse(userDTO, emailAddress, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }

        public bool ValidateTfaCode(string secret, string code)
        {
            return _twoFactorAuthService.ValidateTwoFactorPin(secret, code);
        }

        public async Task<UserDTO?> SetTwoFactorEnabledAsync(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            user.TwoFactorEnabled = true;
            return await _userService.UpdateAsync(user);
        }

        public async Task<UserDTO?> SetTwoFactorDisabledAsync(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            user.TwoFactorEnabled = false;
            return await _userService.UpdateAsync(user);
        }

        public TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            return _twoFactorAuthService.GenerateTfaSetup(issuer, accountTitle, accountSecretKey);
        }

        public async Task<TfaSetupDTO?> EnableTfa(Guid userId, TfaSetupDTO tfaSetupDTO)
        {
            var userDTO = await _userService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            var isValidCode = ValidateTfaCode(userDTO.TwoFactorSecret, tfaSetupDTO.Code);

            if (!isValidCode)
                return null;

            userDTO = await SetTwoFactorEnabledAsync(userId);

            var tfaResponse = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, userDTO.TwoFactorSecret!);
            tfaResponse.IsTfaEnabled = true;

            return tfaResponse;
        }


        public async Task<TfaSetupDTO?> DisableTfa(Guid userId, TfaSetupDTO tfaSetupDTO)
        {
            var userDTO = await _userService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            var isValidCode = ValidateTfaCode(userDTO.TwoFactorSecret, tfaSetupDTO.Code);

            if (!isValidCode)
                return null;

            userDTO = await SetTwoFactorDisabledAsync(userId);

            var tfaResponse = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, userDTO.TwoFactorSecret!);
            tfaResponse.IsTfaEnabled = false;

            return tfaResponse;
        }

        public async Task<RegisterResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (await _userService.AnyAsync(user => user.EmailAddress!.Equals(requestDTO.EmailAddress!)))
                throw new AppException("Email is already used by another user.");

            var userDTO = new UserDTO()
            {
                EmailAddress = requestDTO.EmailAddress,
                PasswordHASH = HashingService.HashPassword(requestDTO.Password!),
                TwoFactorEnabled = false,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                TwoFactorSecret = Guid.NewGuid().ToString().Trim().Replace("-", "").Substring(0, 10)
            };

            userDTO = await _userService.CreateAsync(userDTO);

            _emailService.SendRegistrationEmail(userDTO.EmailAddress!, userDTO.EmailConfirmationToken!);

            userDTO.Password = string.Empty;

            return new RegisterResponseDTO() { User = userDTO, IsRegistrationSuccessful = true };
        }

        public AuthResponseDTO GetAuthResponse(UserDTO user, string emailAddress, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            var expires = DateTime.UtcNow.AddMinutes(_appSettings?.JweTokenMinutesTTL ?? 5);
            var claims = _jwtService.GetClaims(user.Id, emailAddress, expires, tfaChecked, emailVerified);
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

            var user = await _userService.GetByIdAsync(new Guid(userId!));

            var response = GetAuthResponse(user, emailAddress!, tfaChecked: tfaChecked, emailVerified: emailConfirmed);

            return response;
        }

        public bool TokenIsValid(string token)
        {
            var password = _jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);

            return password != null;
        }

        #region Private Methods

        private bool IsRequestValid(LoginRequestDTO requestDTO)
        {
            if (requestDTO is null)
                return false;
            return requestDTO.EmailAddress is not null && requestDTO.Password is not null;
        }

        private async Task<bool> AuthUser(string emailAddress, string password)
        {
            var user = await _userService.GetByEmailAsync(emailAddress);
            if (user is null)
                return false;

            if (HashingService.Verify(password, user.PasswordHASH))
                return true;

            return false;
        }

        public async Task<TfaSetupDTO?> GetTfaSetup(Guid userId)
        {
            var userDTO = await _userService.GetByIdAsync(userId);
            if (userDTO is null)
                return null;

            if (userDTO.TwoFactorSecret is null)
            {
                var newTwoFactorSecret = Guid.NewGuid().ToString().Trim().Replace("-", "").Substring(0, 10);
                userDTO.TwoFactorSecret = newTwoFactorSecret;
                userDTO = await _userService.UpdateAsync(userDTO);
            }

            var result = GenerateTfaSetupDTO("Password Manager", userDTO.EmailAddress!, userDTO.TwoFactorSecret!);
            result.IsTfaEnabled = userDTO.TwoFactorEnabled;

            return result;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var userDTO = await _userService.GetByEmailAsync(email);

            if (userDTO is null) return false;
            if (userDTO.EmailConfirmed)
                return true;
            if (userDTO.EmailConfirmationToken is null) return true;

            if (userDTO.EmailConfirmationToken.Equals(token))
            {
                userDTO.EmailConfirmed = true;
                await _userService.UpdateAsync(userDTO);
                return true;
            }
            return false;
        }

        public async Task<bool> ResendConfirmEmail(string email)
        {
            var userDTO = await _userService.GetByEmailAsync(email);

            if (userDTO is null) return false;

            _emailService.SendRegistrationEmail(email, userDTO.EmailConfirmationToken!);
            return true;
        }

        public async Task<AuthResponseDTO?> LoginTfaAsync(string code, string email, string password)
        {
            if (code is null || email is null || password is null)
                return null;

            var userDTO = await _userService.GetByEmailAsync(email);
            if (userDTO is null) return null;

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(userDTO.TwoFactorSecret!, code!);

            if (!valid)
                return null;

            var response = GetAuthResponse(userDTO, email, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }

        public async Task<AuthResponseDTO?> LoginWithTfaAsync(LoginWithTfaRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (!await AuthUser(requestDTO.EmailAddress!, requestDTO.Password!))
                return null;

            var userDTO = await _userService.GetByEmailAsync(requestDTO.EmailAddress!);
            if (userDTO is null) return null;

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(userDTO.TwoFactorSecret!, requestDTO.Code!);

            if (!valid)
                return null;

            var response = GetAuthResponse(userDTO, userDTO.EmailAddress!, true, userDTO.TwoFactorEnabled, true, userDTO.EmailConfirmed);

            return response;
        }

        public bool ValidateMasterPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            bool hasLowercase = false;
            bool hasUppercase = false;
            bool hasNumber = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsLower(c))
                    hasLowercase = true;
                else if (char.IsUpper(c))
                    hasUppercase = true;
                else if (char.IsDigit(c))
                    hasNumber = true;
                else if ("(!@#$%*".Contains(c))
                    hasSpecialChar = true;
            }

            return hasLowercase && hasUppercase && hasNumber && hasSpecialChar;
        }

        #endregion
    }
}
