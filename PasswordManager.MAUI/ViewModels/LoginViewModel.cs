using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Cryptography;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        string _emailAddress;

        [ObservableProperty]
        string _password;

        IMauiAuthService _authService;
        IMauiSyncService _syncService;

        #endregion

        public LoginViewModel(IMauiAuthService authService, IMauiSyncService syncService)
        {
            Title = "Login";
            _authService = authService;
            _syncService = syncService;
        }

        #region Commands

        [RelayCommand]
        async Task Login()
        {
            var model = new LoginRequestDTO() { EmailAddress = EmailAddress, Password = Password };

            var emailNotConfirmedLabel = Shell.Current.CurrentPage.FindByName<Label>("EmailNotConfirmedError");
            if (emailNotConfirmedLabel is not null)
                emailNotConfirmedLabel.IsVisible = false;

            if (!ValidationHelper.ValidateForm(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            try
            {
                var response = await _authService.LoginAsync(model);

                if (response is null)
                {
                    await AlertService.ShowToast("Wrong credentials.");
                    IsBusy = false;
                    return;
                }

                if (!response.EmailVerified)
                {
                    await AlertService.ShowToast("Email is not confirmed.");
                    if (emailNotConfirmedLabel is not null)
                        emailNotConfirmedLabel.IsVisible = true;
                    IsBusy = false;
                    return;
                }

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
                }

                var userDTO = response.User;
                userDTO.Password = Password;

                ActiveUserService.Instance.Login(userDTO, HashingService.HashSHA256ToString(Password));
                ActiveUserService.Instance.Token = response.JweToken;
                ActiveUserService.Instance.TokenExpirationDateTime = response.ExpirationDateTime;

                if (response.IsTfaEnabled)
                {
                    // Synchronization is done in LoginTfaPage, because full authorization is needed
                    await Shell.Current.GoToAsync(nameof(LoginTfaPage));
                    IsBusy = false;
                    return;
                }

                if (isDataFromServer)
                {
                    var syncResult = await _syncService.DoSync();
                }

                await Shell.Current.GoToAsync($"//Home", true);
                await AlertService.ShowToast("Logged in");
                Password = string.Empty;
            }
            catch (Exception ex)
            {
                await AlertService.ShowToast("Some kind of error occured or wrong credentials.");
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
