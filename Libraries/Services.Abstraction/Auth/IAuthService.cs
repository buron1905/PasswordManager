using Models.DTOs;

namespace Services.Abstraction.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);
        Task<AuthResponseDTO?> LoginTfaAsync(LoginTfaRequestDTO requestDTO);
        Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO);
        Task<AuthResponseDTO?> RefreshTokenAsync(string token);
        Task<AuthResponseDTO?> SetTwoFactorEnabledAsync(Guid userId, string password);
        Task SetTwoFactorDisabledAsync(Guid userId);
        Task<TfaSetupDTO?> GetTfaSetup(string email, string password);
        TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey);
        bool ValidateTfaCode(string secret, string code);
        bool TokenIsValid(string token);
    }
}
