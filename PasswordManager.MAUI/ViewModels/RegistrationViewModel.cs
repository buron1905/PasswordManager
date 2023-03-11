using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        RegisterRequestDTO _model;

        #endregion

        public RegistrationViewModel()
        {
            Title = "Register";

            Model = new RegisterRequestDTO();
        }

        #region Commands

        [RelayCommand]
        async Task Register()
        {
            if (!ValidationHelper.IsFormValid(Model, Shell.Current.CurrentPage))
                return;

            try
            {
                //await DatabaseService.AddUser(newUser);
                //ActiveUserService.Instance.Login(newUser, Model.Password);
                //throw new Exception("Email is already used by another user.");
            }
            catch (Exception ex)
            {
                await PopupService.ShowSnackbar(ex.Message);
                return;
            }

            await PopupService.ShowToast("Successfully registered");
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
        }

        #endregion
    }
}
