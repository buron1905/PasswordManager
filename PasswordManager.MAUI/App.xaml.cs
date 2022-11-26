using PasswordManager.MAUI.Helpers;

namespace PasswordManager.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            TheTheme.SetTheme();
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();
            OnResume();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}