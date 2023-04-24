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

        IMauiAuthService _authService;
        IMauiSyncService _syncService;
        IDataServiceWrapper _dataServiceWrapper;

        #endregion

        public LoginViewModel(IMauiAuthService authService, IMauiSyncService syncService, IDataServiceWrapper dataServiceWrapper)
        {
            Title = "Login";
            _authService = authService;
            _syncService = syncService;
            _dataServiceWrapper = dataServiceWrapper;
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

                if (response is not null)
                {
                    if (!response.EmailVerified)
                    {
                        await AlertService.ShowToast("Email is not confirmed.");
                        if (emailNotConfirmedLabel is not null)
                            emailNotConfirmedLabel.IsVisible = true;
                        IsBusy = false;
                        return;
                    }

                    UserDTO userDTO = null;
                    try
                    {
                        userDTO = await _dataServiceWrapper.UserService.GetByEmailAsync(model.EmailAddress);
                    }
                    catch (Exception)
                    {
                        // user on local device does not exist
                    }

                    ActiveUserService.Instance.Login(userDTO, Password);
                    ActiveUserService.Instance.Token = response.JweToken;

                    if (response.IsTfaEnabled)
                    {
                        await Shell.Current.GoToAsync(nameof(LoginTfaPage)); // v tomto synchronizovat
                        IsBusy = false;
                        return;
                    }

                    if (response.JweToken is not null)
                    {
                        var result = await _syncService.DoSync();
                    }

                    //await Shell.Current.GoToAsync(nameof(LoadingPage));

                    await Shell.Current.GoToAsync($"//Home", true);
                    await AlertService.ShowToast("Logged in");
                    Password = string.Empty;
                }
                else
                {
                    await AlertService.ShowToast("Wrong credentials.");
                }
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
