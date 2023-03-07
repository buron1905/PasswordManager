using Models.DTOs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Views;
using System.Windows.Input;

namespace PasswordManager.MAUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        public LoginViewModel()
        {
            Title = "Password Manager";

            LoginCommand = new AsyncCommand(Login);
            RegistrationCommand = new AsyncCommand(Registration);

            Model = new LoginRequestDTO();
        }

        LoginRequestDTO _loginRequestDTO;
        public LoginRequestDTO Model
        {
            get => _loginRequestDTO;
            set => SetProperty(ref _loginRequestDTO, value);
        }

        private async Task Registration()
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private async Task Login()
        {
            if (!ValidationHelper.IsFormValid(Model, Shell.Current.CurrentPage))
                return;

            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            //if (await LoginService.Login(Model.EmailAddress, Model.Password))
            //{
            //    await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            //}
            //else
            //{
            //    await PopupService.ShowError("Error", "Wrong credentials.");
            //}
        }

    }
}
