using Google.Authenticator;
using Models.DTOs;
using Services.Abstraction.Auth;

namespace Services.Auth
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient)
        {
            var tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN(accountSecretKey, twoFactorCodeFromClient);
        }

        public TfaSetupDTO GenerateTfaSetup(string issuer, string accountTitleNoSpaces, string accountSecretKey, bool secretIsBase32 = false)
        {
            var tfa = new TwoFactorAuthenticator();
            var setupCode = tfa.GenerateSetupCode(issuer, accountTitleNoSpaces, accountSecretKey, secretIsBase32);
            return new TfaSetupDTO() { AuthenticatorKey = setupCode.ManualEntryKey, QrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl };
        }

        public string GetCurrentPIN(string accountSecretKey)
        {
            var tfa = new TwoFactorAuthenticator();
            return tfa.GetCurrentPIN(accountSecretKey);
        }
    }
}
