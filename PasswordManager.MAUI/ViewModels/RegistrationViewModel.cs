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
        IConnectivity _connectivity;

        #endregion

        public RegistrationViewModel(IAuthService authService, IConnectivity connectivity)
        {
            Title = "Register";
            _authService = authService;
            _connectivity = connectivity;
        }

        #region Commands

        [RelayCommand]
        async Task Register()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await PopupService.ShowToast("Registration must be online");
            }

            var model = new RegisterRequestDTO() { EmailAddress = EmailAddress, Password = Password, ConfirmPassword = ConfirmPassword };
            var modelDTO = new UserDTO() { Id = Guid.NewGuid(), EmailAddress = EmailAddress, Password = Password };

            if (!ValidationHelper.IsFormValid(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            try
            {
                var result = await _authService.RegisterAsync(model);

                if (result is not null)
                {
                    ActiveUserService.Instance.Login(modelDTO, Password);
                    await PopupService.ShowToast("Successfully registered");
                    await Shell.Current.GoToAsync($"{nameof(RegistrationSuccessfulPage)}");

                    EmailAddress = Password = ConfirmPassword = string.Empty;
                }
                else
                {
                    await PopupService.ShowToast("Error");
                }
            }
            catch (Exception ex)
            {
                await PopupService.ShowToast(ex.Message);
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
