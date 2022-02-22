using Microsoft.Maui.Controls;
using PasswordManager.Services;
using PasswordManager.Views;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;
using MvvmHelpers;

namespace PasswordManager.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(Login);
            RegistrationCommand = new Command(Registration);
        }

        string _email = "";
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        private async void Registration()
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                
                PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            if (await LoginService.Login(Email, Password))
            {
                await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            }
            else
            {
                PopupService.ShowError("Error", "Invalid login. Please try again.");
            }
        }

    }
}
