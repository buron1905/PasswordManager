using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using System.Windows.Input;

namespace PasswordManager.MAUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        public LoginViewModel()
        {
            Title = "Login";

            LoginCommand = new AsyncCommand(Login);
            RegistrationCommand = new AsyncCommand(Registration);
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

        private async Task Registration()
        {
            //await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await PopupService.ShowError("Error", "Fields must not be empty");
                return;
            }

            if (await LoginService.Login(Email, Password))
            {
                //await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            }
            else
            {
                await PopupService.ShowError("Error", "Invalid login. Please try again.");
            }
        }

    }
}
