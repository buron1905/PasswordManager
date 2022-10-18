using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.TMP;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AuthService(IDataServiceWrapper dataServiceWrapper)
        {
            _dataServiceWrapper = dataServiceWrapper;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            var user = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.Email!);

            if (!BCrypt.Net.BCrypt.Verify(requestDTO.Password, user.PasswordHASH))
                return null;

            var tokenString = new JwtService().GenerateJweToken(requestDTO.Password!, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);
            var refreshTokenString = new JwtService().GenerateJwtToken(requestDTO.Password!, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);

            return new AuthResponseDTO { 
                Token = tokenString, 
                RefreshToken = refreshTokenString,
                ExpirationDateTime = DateTime.Now.AddMinutes(30)
            };
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            requestDTO.PasswordHASH = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);

            var user = await _dataServiceWrapper.UserService.CreateAsync(requestDTO);

            var tokenString = new JwtService().GenerateJweToken(requestDTO.Password!, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);
            //var refreshTokenString = new JwtService().GenerateJwtToken(requestDTO.Password!, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);

            return new AuthResponseDTO
            {
                Token = tokenString,
                RefreshToken = refreshTokenString,
                ExpirationDateTime = DateTime.Now.AddMinutes(30)
            };
        }

        public bool TokenIsValid(string token)
        {
            var password = new JwtService().ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);

            return password != null;
        }

        private bool IsRequestValid(LoginRequestDTO requestDTO)
        {
            if (requestDTO is null)
                return false;
            return requestDTO.Email is not null || requestDTO.Password is not null;
        }
    }
}
