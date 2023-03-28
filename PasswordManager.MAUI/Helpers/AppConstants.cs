namespace PasswordManager.MAUI.Helpers
{
    public class AppConstants
    {
        // Base
        public const string ApiUrl = "https://10.0.2.2:5001/api/";
        //public const string ApiUrl = "https://localhost:5001/api/";

        // Auth
        public const string LoginSuffix = "Auth/login";
        public const string RegisterSuffix = "Auth/register";
        public const string RefreshTokenSuffix = "Auth/refresh-token";

        // Passwords
        public const string PasswordsSuffix = "Passwords";
        public const string PasswordsGeneratorSuffix = "Passwords/generator";

        // Settings
        public const string SettingsSuffix = "Settings";

    }
}
