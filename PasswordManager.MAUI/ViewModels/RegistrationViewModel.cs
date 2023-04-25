using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Exceptions;

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
        IMauiSyncService _syncService;
        IConnectivity _connectivity;

        #endregion

        public RegistrationViewModel(IMauiAuthService authService, IMauiSyncService syncService, IConnectivity connectivity)
        {
            Title = "Register";
            _authService = authService;
            _syncService = syncService;
            _connectivity = connectivity;
        }

        #region Commands

        [RelayCommand]
        async Task Register()
        {
            var registrationRequestDTO = new RegisterRequestDTO() { EmailAddress = EmailAddress, Password = Password, ConfirmPassword = ConfirmPassword };

            var usedEmailLabel = Shell.Current.CurrentPage.FindByName<Label>("UsedEmailError");
            if (usedEmailLabel is not null)
                usedEmailLabel.IsVisible = false;

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await AlertService.ShowToast("Registration must be online", CommunityToolkit.Maui.Core.ToastDuration.Long);
                ValidationHelper.ValidateForm(registrationRequestDTO, Shell.Current.CurrentPage);
                return;
            }

            if (!ValidationHelper.ValidateForm(registrationRequestDTO, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            try
            {
                var result = await _authService.RegisterAsync(registrationRequestDTO);

                if (result is not null && result.IsRegistrationSuccessful)
                {
                    var syncUserResult = await _syncService.SyncExistingAndNewUser(result.User);
                    if (syncUserResult is not null && syncUserResult.SyncSuccessful)
                    {
                        await AlertService.ShowToast("Successfully registered");
                        await Shell.Current.GoToAsync($"{nameof(RegistrationSuccessfulPage)}");

                        EmailAddress = Password = ConfirmPassword = string.Empty;
                    }
                }
                else
                {
                    if (usedEmailLabel is not null)
                        usedEmailLabel.IsVisible = true;
                    await AlertService.ShowToast("Registration Unsuccessful");
                }
            }
            catch (AppException ex)
            {
                await AlertService.ShowToast($"{ex.Message}");
            }
            catch
            {
                await AlertService.ShowToast($"Unhandled Error");
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
