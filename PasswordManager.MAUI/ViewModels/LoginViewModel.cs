using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;

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
        IDataServiceWrapper _dataServiceWrapper;

        #endregion

        public LoginViewModel(IAuthService authService, IDataServiceWrapper dataServiceWrapper)
        {
            Title = "Login";
            _authService = authService;
            _dataServiceWrapper = dataServiceWrapper;
        }

        #region Commands

        [RelayCommand]
        async Task Login()
        {
            var model = new LoginRequestDTO() { EmailAddress = EmailAddress, Password = Password };

            if (!ValidationHelper.IsFormValid(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            try
            {
                var response = await _authService.LoginAsync(model);

                if (response is not null)
                {
                    await Shell.Current.GoToAsync(nameof(LoadingPage));

                    var user = await _dataServiceWrapper.UserService.GetByEmailAsync(model.EmailAddress);
                    ActiveUserService.Instance.Login(user, Password);

                    await Shell.Current.GoToAsync($"//Home", true);
                    await PopupService.ShowToast("Logged in");
                    Password = string.Empty;
                }
                else
                {
                    await PopupService.ShowToast("Wrong credentials.");
                }
            }
            catch (Exception ex)
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
