using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoginTfaViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _code;

        IAuthService _authService;

        #endregion

        public LoginTfaViewModel(IAuthService authService)
        {
            Title = "Two-Factor Authentication";
            _authService = authService;
        }

        #region Commands

        [RelayCommand]
        async Task Verify()
        {
            IsBusy = true;
            var response = await _authService.LoginTfaAsync(new LoginTfaRequestDTO() { Code = Code, Token = ActiveUserService.Instance.Token });

            if (response is null)
            {
                await AlertService.ShowToast("Wrong Code.");
                IsBusy = false;
                return;
            }

            await Shell.Current.GoToAsync($"//Home", true);
            await AlertService.ShowToast("Logged in");
            IsBusy = false;
        }

        [RelayCommand]
        async Task Cancel()
        {
            ActiveUserService.Instance.Logout();
            await Shell.Current.GoToAsync($"..", true);
        }

        #endregion

    }
}
