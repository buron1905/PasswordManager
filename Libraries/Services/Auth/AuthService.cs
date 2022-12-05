using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using Services.TMP;
using System.Security.Claims;

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

            if (!await AuthUser(requestDTO.EmailAddress!, requestDTO.Password!))
                return null;

            //get user guid
            var userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(requestDTO.EmailAddress!);

            return GetAuthResponse(userDTO.Id, requestDTO.EmailAddress!, requestDTO.Password!);
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsRequestValid(requestDTO))
                return null;

            requestDTO.PasswordHASH = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);

            if (await _repositoryWrapper.UserRepository.AnyAsync(user => user.EmailAddress.Equals(requestDTO.EmailAddress!)))
                throw new AppException("Email is already used by another user.");

            var userDTO = await _dataServiceWrapper.UserService.CreateAsync(requestDTO);

            return GetAuthResponse(userDTO.Id, requestDTO.EmailAddress!, requestDTO.Password!);
        }

        private AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password)
        {
            var expires = DateTime.UtcNow.AddMinutes(_appSettings.JweTokenMinutesTTL);
            var claims = _jwtService.GetClaims(userId, emailAddress, password, expires);
            var tokenString = _jwtService.GenerateJweToken(claims, JWTKeys._privateSigningKey, JWTKeys._publicEncryptionKey, expires);

            return new AuthResponseDTO
            {
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

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHASH))
                return false;
            //throw new PasswordNotFoundException();

            return true;
        }

    }
}
