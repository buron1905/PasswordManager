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

            return GetAuthResponse(user, requestDTO.Password!);
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            requestDTO.PasswordHASH = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);

            var user = requestDTO.Adapt<User>();
            _repositoryWrapper.UserRepository.Create(user);
            await _repositoryWrapper.SaveChangesAsync();

            return GetAuthResponse(user, requestDTO.Password!);
        }

        private AuthResponseDTO GetAuthResponse(User user, string password)
        {
            var tokenString = _jwtService.GenerateJweToken(password, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey);

            return new AuthResponseDTO
            {
                JweToken = tokenString,
                ExpirationDateTime = DateTime.Now.AddMinutes(_appSettings.JweTokenMinutesTTL)
            };
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

    }
}
