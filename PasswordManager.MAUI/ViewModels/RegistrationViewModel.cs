using Models.DTOs;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using System.Windows.Input;

namespace PasswordManager.MAUI.ViewModels
{
    public class RegistrationViewModel : BaseWithValidationViewModel<RegisterRequestDTO>
    {
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public RegistrationViewModel()
        {
            Title = "Registration";

            RegisterCommand = new AsyncCommand(Register);
            LoginCommand = new AsyncCommand(Login);

            Model = new RegisterRequestDTO();
        }

        private async Task Login()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async Task Register()
        {
            if (!IsFormValid())
                return;

            try
            {
                //await DatabaseService.AddUser(newUser);
                //ActiveUserService.Instance.Login(newUser, Model.Password);
            }
            catch (Exception ex)
            {
                await PopupService.ShowError("Error", $"{ex.Message}");
                return;
            }

            //await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
        }
    }
}
