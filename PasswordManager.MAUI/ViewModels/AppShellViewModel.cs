using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        public AppShellViewModel()
        {
            Title = "Password Manager";
        }

        #region Commands

        [RelayCommand]
        async void Logout()
        {
            //if (Preferences.ContainsKey(nameof(App.UserDetails)))
            //{
            //    Preferences.Remove(nameof(App.UserDetails));
            //}
            ActiveUserService.Instance.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        #endregion
    }
}
