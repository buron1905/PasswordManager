namespace PasswordManager.MAUI.Helpers
{

    // Theme not yet used due to BUG in NET 7...

    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                case Theme.Unspecified:
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
                case Theme.Light:
                    Application.Current.UserAppTheme = AppTheme.Light;
                    break;
                case Theme.Dark:
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    break;
            }
        }
    }
}
