#if __ANDROID__
using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif
using PasswordManager.MAUI.Handlers;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Timer = System.Timers.Timer;

namespace PasswordManager.MAUI
{
    public partial class App : Application
    {
        Timer _idleTimer = new Timer(60000 * 7); // 5 minutes

        public App()
        {
            InitializeComponent();
            ModifyCustomControls();

            MainPage = new AppShell();

            _idleTimer.Elapsed += Idleimer_Elapsed;
            _idleTimer.Start();
        }

        private void ModifyCustomControls()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderLine", (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                    (handler.PlatformView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Transparent.ToAndroid());
#endif
                }
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoUnderLine", (handler, view) =>
            {
                if (view is BorderlessEditor)
                {
#if ANDROID
                    (handler.PlatformView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Transparent.ToAndroid());
#endif
                }
            });
        }

        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
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

        async void Idleimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MainThread.IsMainThread)
            {
                if (Shell.Current.CurrentPage is not LoginPage && Shell.Current.CurrentPage is not RegistrationPage)
                {
                    ActiveUserService.Instance.Logout();
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            else
            {
                if (Shell.Current.CurrentPage is not LoginPage && Shell.Current.CurrentPage is not RegistrationPage)
                {
                    ActiveUserService.Instance.Logout();
                    MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}"));
                }
            }
        }

        public void ResetIdleTimer()
        {
            _idleTimer.Stop();
            _idleTimer.Start();
        }

    }
}