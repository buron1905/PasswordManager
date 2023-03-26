﻿using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
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
        private readonly AppSettings? _appSettings;

        public AuthService(IDataServiceWrapper dataServiceWrapper, IRepositoryWrapper repositoryWrapper, ITwoFactorAuthService twoFactorAuthService, IJwtService jwtService, IOptions<AppSettings>? appSettings = null)
        {
            _dataServiceWrapper = dataServiceWrapper;
            _repositoryWrapper = repositoryWrapper;
            _twoFactorAuthService = twoFactorAuthService;
            _jwtService = jwtService;
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
            var response = GetAuthResponse(userDTO.Id, requestDTO.EmailAddress!, requestDTO.Password!, true, userDTO.TwoFactorEnabled, !userDTO.TwoFactorEnabled);

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

            var valid = _twoFactorAuthService.ValidateTwoFactorPin(password, requestDTO.Code!);

            if (!valid)
                return null;

            var emailAddress = JwtService.GetUserEmailFromClaims(claims);

            var response = GetAuthResponse(userId, emailAddress, password, true, true, true);

            return response;
        }

        public bool ValidateTfaCode(string secret, string code)
        {
            return _twoFactorAuthService.ValidateTwoFactorPin(secret, code);
        }

        public async Task<AuthResponseDTO?> SetTwoFactorEnabledAsync(Guid userId, string password)
        {
            var user = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            //if (password is not null)
            //{
            //    secret = Encoding.Unicode.GetString(await EncryptionService.EncryptAsync(secret, password));
            //}

            user.TwoFactorEnabled = true;
            //user.TwoFactorSecret = secret;
            var result = await _dataServiceWrapper.UserService.UpdateAsync(userId, user);

            var response = GetAuthResponse(userId, user.EmailAddress, password, true, true, true);

            return response;
        }

        public async Task SetTwoFactorDisabledAsync(Guid userId)
        {
            var user = await _dataServiceWrapper.UserService.GetByIdAsync(userId);
            user.TwoFactorEnabled = false;
            await _dataServiceWrapper.UserService.UpdateAsync(userId, user);
        }

        public TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            return _twoFactorAuthService.GenerateTfaSetup(issuer, accountTitle, accountSecretKey);
        }

        //private async Task<string?> GetTfaSecret(Guid userId, string password)
        //{
        //    var secretEncrypted = (await _dataServiceWrapper.UserService.GetByIdAsync(userId)).TwoFactorSecret;
        //    if (secretEncrypted is null)
        //        return null;

        //    var secretDecrypted = await EncryptionService.DecryptAsync(Encoding.Unicode.GetBytes(secretEncrypted),
        //        password);

        //    return secretDecrypted;
        //}

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            if (await _repositoryWrapper.UserRepository.AnyAsync(user => user.EmailAddress.Equals(requestDTO.EmailAddress!)))
                throw new AppException("Email is already used by another user.");

            requestDTO.PasswordHASH = HashingService.HashPassword(requestDTO.Password);

            var userDTO = await _dataServiceWrapper.UserService.CreateAsync(requestDTO);

            return GetAuthResponse(userDTO.Id, requestDTO.EmailAddress!, requestDTO.Password!);
        }

        private AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true)
        {
            var expires = DateTime.UtcNow.AddMinutes(_appSettings?.JweTokenMinutesTTL ?? 5);
            var claims = _jwtService.GetClaims(userId, emailAddress, password, expires, tfaChecked);
            var tokenString = _jwtService.GenerateJweToken(claims, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey, expires);

            return new AuthResponseDTO
            {
                IsAuthSuccessful = isAuthSuccessful,
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

            if (!await AuthUser(emailAddress!, password!))
                return null;

            var response = GetAuthResponse(new Guid(userId!), emailAddress!, password!);

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

        public async Task<TfaSetupDTO?> GetTfaSetup(string email, string password)
        {
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(email);
            if (userDTO is null)
                return null;

            var result = GenerateTfaSetupDTO("Password Manager", email!, password!);
            result.IsTfaEnabled = userDTO.TwoFactorEnabled;

            return result;
        }
    }
}
