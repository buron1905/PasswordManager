using Models.DTOs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using System.Windows.Input;

namespace PasswordManager.MAUI.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
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

        RegisterRequestDTO _registerRequestDTO;
        public RegisterRequestDTO Model
        {
            get => _registerRequestDTO;
            set => SetProperty(ref _registerRequestDTO, value);
        }

        private async Task Login()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async Task Register()
        {
            if (!ValidationHelper.IsFormValid(Model, Shell.Current.CurrentPage))
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
