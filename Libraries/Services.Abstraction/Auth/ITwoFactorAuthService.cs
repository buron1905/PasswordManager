using Models.DTOs;

namespace Services.Abstraction.Auth
{
    public interface ITwoFactorAuthService
    {
        bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient);
        string GetCurrentPIN(string accountSecretKey);
        TfaSetupDTO GenerateTfaSetup(string issuer, string accountTitleNoSpaces, string accountSecretKey, bool secretIsBase32 = false);
    }
}
