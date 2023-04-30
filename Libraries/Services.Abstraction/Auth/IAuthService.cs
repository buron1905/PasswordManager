using Models.DTOs;

namespace Services.Abstraction.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);
        Task<AuthResponseDTO?> LoginTfaAsync(LoginTfaRequestDTO requestDTO);
        Task<AuthResponseDTO?> LoginTfaAsync(string code, string email, string password);
        Task<AuthResponseDTO?> LoginWithTfaAsync(LoginWithTfaRequestDTO requestDTO);
        Task<RegisterResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO);
        Task<AuthResponseDTO?> RefreshTokenAsync(string token);
        AuthResponseDTO GetAuthResponse(UserDTO user, string emailAddress, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true);
        Task<UserDTO?> SetTwoFactorEnabledAsync(Guid userId);
        Task<bool> ConfirmEmailAsync(string email, string token);
        Task<UserDTO?> SetTwoFactorDisabledAsync(Guid userId);
        Task<TfaSetupDTO?> GetTfaSetup(Guid userId);
        Task<TfaSetupDTO?> EnableTfa(Guid userId, TfaSetupDTO tfaSetupDTO);
        Task<TfaSetupDTO?> DisableTfa(Guid userId, TfaSetupDTO tfaSetupDTO);
        TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey);
        bool ValidateTfaCode(string secret, string code);
        bool TokenIsValid(string token);
        Task<bool> ResendConfirmEmail(string email);
        bool ValidateMasterPassword(string password);

    }
}
