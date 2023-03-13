using Models.DTOs;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.Services
{
    public class AuthService : IAuthService
    {
        public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public bool TokenIsValid(string token)
        {
            throw new NotImplementedException();
        }
    }
}
