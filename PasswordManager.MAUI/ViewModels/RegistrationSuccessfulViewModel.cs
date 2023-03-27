using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class RegistrationSuccessfulViewModel : BaseViewModel
    {
        #region Properties
        #endregion

        public RegistrationSuccessfulViewModel()
        {
            Title = "Registration Successful";
        }

        #region Commands

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
        }

        #endregion
    }
}
