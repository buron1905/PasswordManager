using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoginTfaViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _code;

        IMauiAuthService _authService;
        IMauiSyncService _syncService;

        #endregion

        public LoginTfaViewModel(IMauiAuthService authService, IMauiSyncService syncService)
        {
            Title = "Two-Factor Authentication";
            _authService = authService;
            _syncService = syncService;
        }

        #region Commands

        [RelayCommand]
        async Task Verify()
        {
            IsBusy = true;
            var response = await _authService.LoginWithTfaAsync(new LoginWithTfaRequestDTO() { Code = Code, EmailAddress = ActiveUserService.Instance.ActiveUser.EmailAddress, Password = ActiveUserService.Instance.CipherKey });

            if (response is null)
            {
                await AlertService.ShowToast("Wrong Code.");
                IsBusy = false;
                return;
            }

            ActiveUserService.Instance.Login(response.User, ActiveUserService.Instance.CipherKey);
            ActiveUserService.Instance.Token = response.JweToken;
            ActiveUserService.Instance.TokenExpirationDateTime = response.ExpirationDateTime;

            bool isDataFromServer = response.JweToken is not null;
            if (isDataFromServer)
            {
                var syncUserResult = await _syncService.SyncExistingAndNewUser(response.User);
                if (syncUserResult is null || !syncUserResult.SyncSuccessful)
                {
                    await AlertService.ShowToast("Error syncing user information. Try again or restart the app.", CommunityToolkit.Maui.Core.ToastDuration.Long);
                    IsBusy = false;
                    return;
                }

                var syncResult = await _syncService.DoSync();
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
