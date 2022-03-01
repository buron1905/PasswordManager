using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;
using Command = MvvmHelpers.Commands.Command;
using MvvmHelpers;
using PasswordManager.Views;

namespace PasswordManager.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public RegistrationViewModel()
        {
            Title = "Registration";

            RegisterCommand = new Command(Register);
            LoginCommand = new Command(Login);
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

        string _passwordAgain = "";
        public string PasswordAgain
        {
            get => _passwordAgain;
            set => SetProperty(ref _passwordAgain, value);
        }

        private async void Login()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await PopupService.ShowError("Error", "Following fields must not be empty: email, master password");
                return;
            }

            if(Password != PasswordAgain)
            {
                await PopupService.ShowError("Error", "Entered passwords don't match");
                return;
            }

            User newUser = new User();
            newUser.Email = Email;
            newUser.PasswordHASH = HashingService.HashSHA512ToString(Password);

            try
            {
                await DatabaseService.AddUser(newUser);
                ActiveUserService.Instance.Login(newUser, Password);
            }
            catch (Exception ex)
            {
                await PopupService.ShowError("Error", $"{ex.Message}");
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
        }
    }
}
