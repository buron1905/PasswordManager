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

namespace PasswordManager.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel()
        {
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


        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        private async void Login()
        {
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync(true);
        }

        private async void Register()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields must not be empty");
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
                PopupService.ShowError("Error", $"{ex.Message}");
                return;
            }

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.PasswordsListPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await Task.Delay(100);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
        }
    }
}
