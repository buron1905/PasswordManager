using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _emailAddress;

        [ObservableProperty]
        string _password;

        [ObservableProperty]
        string _confirmPassword;

        IAuthService _authService;

        #endregion

        public RegistrationViewModel(IAuthService authService)
        {
            Title = "Register";
            _authService = authService;
        }

        #region Commands

        [RelayCommand]
        async Task Register()
        {
            var model = new RegisterRequestDTO() { EmailAddress = EmailAddress, Password = Password, ConfirmPassword = ConfirmPassword };

            if (!ValidationHelper.IsFormValid(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            try
            {
                //await DatabaseService.AddUser(newUser);
                //ActiveUserService.Instance.Login(newUser, Model.Password);
                //throw new Exception("Email is already used by another user.");
                await _authService.RegisterAsync(model);

                await PopupService.ShowToast("Successfully registered");
                await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");

                EmailAddress = Password = ConfirmPassword = string.Empty;
            }
            catch (Exception ex)
            {
                await PopupService.ShowSnackbar(ex.Message);
            }

            IsBusy = false;
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
        }

        #endregion
    }
}
