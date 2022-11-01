using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using Services.TMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Auth
{
    /// <summary>
    /// Service is used for login and registration
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IDataServiceWrapper _dataServiceWrapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IJwtService _jwtService;
        private readonly AppSettings _appSettings;

        public AuthService(IDataServiceWrapper dataServiceWrapper, IRepositoryWrapper repositoryWrapper, IJwtService jwtService, IOptions<AppSettings> appSettings)
        {
            _dataServiceWrapper = dataServiceWrapper;
            _repositoryWrapper = repositoryWrapper;
            _jwtService = jwtService;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            //var user = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.Email!);
            var user = await _repositoryWrapper.UserRepository.FindSingleOrDefaultByCondition(user => user.Email.Equals(requestDTO.Email!));
            if (user is null)
                throw new UserNotFoundException(requestDTO.Email!);

            if (!BCrypt.Net.BCrypt.Verify(requestDTO.Password, user.PasswordHASH))
                return null;

            return await SetupAuthResponseAsync(user, requestDTO.Password!);
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            requestDTO.PasswordHASH = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);

            var user = requestDTO.Adapt<User>();
            _repositoryWrapper.UserRepository.Create(user);

            return await SetupAuthResponseAsync(user, requestDTO.Password!);
        }

        private async Task<AuthResponseDTO> SetupAuthResponseAsync(User user, string password)
        {
            var tokenString = _jwtService.GenerateJweToken(password, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);
            var refreshToken = _jwtService.GenerateRefreshToken(await GetUniqueTokenAsync(), DateTime.UtcNow.AddDays(_appSettings.RefreshTokenDaysTTL));
            user.RefreshTokens.Add(refreshToken);

            RemoveOldRefreshTokens();
            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.SaveChangesAsync();

            return new AuthResponseDTO
            {
                JweToken = tokenString,
                RefreshToken = refreshToken.Token,
                ExpirationDateTime = DateTime.Now.AddMinutes(_appSettings.JweTokenMinutesTTL)
            };
        }

        public async Task<AuthResponseDTO?> RefreshToken(string token, string password)
        {
            var user = await _repositoryWrapper.UserRepository.FindSingleOrDefaultByCondition(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user is null)
                throw new AppException("Invalid token");

            var refreshToken = await _repositoryWrapper.RefreshTokenRepository.FindSingleOrDefaultByCondition(x => x.Token == token);
            if (refreshToken is null)
                throw new RefreshTokenNotFoundException();

            if (refreshToken is not null && refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, user, $"Attempted reuse of revoked ancestor token: {token}");
                _repositoryWrapper.UserRepository.Update(user);
                await _repositoryWrapper.SaveChangesAsync();
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            var response = await SetupAuthResponseAsync(user, password);
            RevokeRefreshToken(refreshToken, "Replaced by new token", response.RefreshToken);
            await _repositoryWrapper.SaveChangesAsync(); // second save...

            return response;
        }
        
        public async Task<bool> RevokeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;
            
            var refreshToken = await _repositoryWrapper.RefreshTokenRepository.FindSingleOrDefaultByCondition(u => u.Token == token);

            if (refreshToken is not null && !refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, "Revoked without replacement");
            await _repositoryWrapper.SaveChangesAsync();

            return true;
        }
        
        public bool TokenIsValid(string token)
        {
            var password = new JwtService().ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);

            return password != null;
        }

        // helper methods
        
        private bool IsRequestValid(LoginRequestDTO requestDTO)
        {
            if (requestDTO is null)
                return false;
            return requestDTO.Email is not null || requestDTO.Password is not null;
        }

        /// <summary>
        /// Replaces old refresh token with a new one.
        /// </summary>
        /// <returns>New Refresh token</returns>
        private async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken)
        {
            var newRefreshToken = _jwtService.GenerateRefreshToken(await GetUniqueTokenAsync(), DateTime.UtcNow.AddDays(_appSettings.RefreshTokenDaysTTL));
            
            RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
            
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens()
        {
            // nebo jen pro toho Usera
            //user.RefreshTokens.RemoveAll(); // RefreshTokens by musely byt List<>
            _repositoryWrapper.RefreshTokenRepository.DeleteAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenDaysTTL) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, reason);
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        private async Task<string> GetUniqueTokenAsync()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            
            // ensure token is unique by checking against db
            var tokenIsUnique = await _dataServiceWrapper.RefreshTokenService.AnyAsync(t => t.Token == token);

            if (!tokenIsUnique)
                return await GetUniqueTokenAsync();

            return token;
        }
    }
}
