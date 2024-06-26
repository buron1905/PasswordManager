﻿namespace PasswordManager.MAUI.Helpers
{
    public class AppConstants
    {
        // Base
        public const string ApiUrl = "https://passwordmanagerwebapi.azurewebsites.net/api/";

        // Auth
        public const string LoginSuffix = "Auth/login/";
        public const string RegisterSuffix = "Auth/register/";
        public const string RefreshTokenSuffix = "Auth/refresh-token/";

        //// 2FA
        public const string LoginWithTfaSuffix = "Auth/tfa-login/";
        public const string LoginTfaSuffix = "Auth/tfa-login-with-token/";
        public const string TfaSetupSuffix = "Auth/tfa-setup/";

        // Passwords
        public const string PasswordsSuffix = "Passwords/{0}";
        public const string PasswordsGeneratorSuffix = "Passwords/generator/";

        // Settings
        public const string SettingsSuffix = "Settings/";

        // Sync
        public const string SyncSuffix = "Sync/";

    }
}
