using Models.DTOs;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using System.Windows.Input;

namespace PasswordManager.MAUI.ViewModels
{
    public class LoginViewModel : BaseWithValidationViewModel<LoginRequestDTO>
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get; }

        public LoginViewModel()
        {
            Title = "Login";

            LoginCommand = new AsyncCommand(Login);
            RegistrationCommand = new AsyncCommand(Registration);

            Model = new LoginRequestDTO();
        }

        private async Task Registration()
        {
            //await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private async Task Login()
        {
            if (!IsFormValid())
                return;

            if (await LoginService.Login(Model.EmailAddress, Model.Password))
            {
                //await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            }
            else
            {
                await PopupService.ShowError("Error", "Wrong credentials.");
            }
        }

    }
}
