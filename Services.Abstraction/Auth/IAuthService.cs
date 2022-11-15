using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);
        Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO);
        Task<AuthResponseDTO?> RefreshTokenAsync(string token);
        bool TokenIsValid(string token);
    }
}
