namespace PasswordManager.MAUI.Helpers
{
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

            //Application.Current.Resources.MergedDictionaries
            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/theming/theming

            //var e = DependencyService.Get<IEnvironment>();
            //if (App.Current.RequestedTheme == OSAppTheme.Dark)
            //{
            //    e?.SetStatusBarColor(Color.Black, false);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.Black;
            //        nav.BarTextColor = Color.White;
            //    }
            //}
            //else
            //{
            //    e?.SetStatusBarColor(Color.White, true);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.White;
            //        nav.BarTextColor = Color.Black;
            //    }
            //}
        }
    }
}
