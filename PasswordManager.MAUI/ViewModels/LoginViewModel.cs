using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _emailAddress;

        [ObservableProperty]
        string _password;

        IAuthService _authService;

        #endregion

        public LoginViewModel(IAuthService authService)
        {
            Title = "Login";
            _authService = authService;
        }

        #region Commands

        [RelayCommand]
        async Task Login()
        {
            var model = new LoginRequestDTO() { EmailAddress = EmailAddress, Password = Password };

            if (!ValidationHelper.IsFormValid(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            //var response = await _authService.LoginAsync(model);

            //if (response is not null)

            if (await LoginService.Login(model.EmailAddress, model.Password))
            {
                await Shell.Current.GoToAsync(nameof(LoadingPage));
                await Shell.Current.GoToAsync($"//Home", true);
                await PopupService.ShowToast("Logged in");
                Password = string.Empty;
            }
            else
            {
                await PopupService.ShowToast("Wrong credentials.");
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task GoToRegister()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}", true);
        }

        #endregion
    }
}
