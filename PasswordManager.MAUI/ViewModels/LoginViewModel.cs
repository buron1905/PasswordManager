using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        LoginRequestDTO _model;

        #endregion

        public LoginViewModel()
        {
            Title = "Login";

            Model = new LoginRequestDTO();
        }

        #region Commands

        [RelayCommand]
        async Task Login()
        {
            if (!ValidationHelper.IsFormValid(Model, Shell.Current.CurrentPage))
                return;

            if (await LoginService.Login(Model.EmailAddress, Model.Password))
            {

                await Shell.Current.GoToAsync($"//Home", true);
                Model.Password = string.Empty;
            }
            else
            {
                await PopupService.ShowToast("Wrong credentials.");
            }
        }

        [RelayCommand]
        async Task GoToRegister()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}", true);
        }

        #endregion
    }
}
