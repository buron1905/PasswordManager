using Models.DTOs;

namespace Services.Abstraction.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);
        Task<AuthResponseDTO?> LoginTfaAsync(LoginTfaRequestDTO requestDTO);
        Task<AuthResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO);
        Task<AuthResponseDTO?> RefreshTokenAsync(string token);
        AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true);
        Task<UserDTO?> SetTwoFactorEnabledAsync(Guid userId, string password);
        Task<bool> ConfirmEmailAsync(string email, string token);
        Task<UserDTO?> SetTwoFactorDisabledAsync(Guid userId);
        Task<TfaSetupDTO?> GetTfaSetup(Guid userId, string password);
        Task<TfaSetupDTO?> EnableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO);
        Task<TfaSetupDTO?> DisableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO);
        Task<string> DecryptString(string password, string textEncrypted);
        TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey);
        bool ValidateTfaCode(string secret, string code);
        bool TokenIsValid(string token);
        Task ResendConfirmEmail(string email);
    }
}
