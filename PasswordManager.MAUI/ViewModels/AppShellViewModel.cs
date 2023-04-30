using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using PasswordManager.MAUI.Views.Controls;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        #region Properties
        #endregion

        public AppShellViewModel()
        {
            Title = "Password Manager";
        }

        #region Commands

        [RelayCommand]
        async void Logout()
        {
            ActiveUserService.Instance.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        [RelayCommand]
        async void GoToWeb()
        {
            try
            {
                await Browser.Default.OpenAsync(new Uri("https://passwordmanagerwebapi.azurewebsites.net/"), BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                await AlertService.ShowToast("Error with opening URL");
            }
        }

        #endregion

        #region Methods

        public static void FlyoutHeaderRefresh()
        {
            if (Shell.Current is not null)
                Shell.Current.FlyoutHeader = new FlyoutHeader();
        }

        #endregion
    }
}
