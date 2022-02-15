using Microsoft.Maui.Controls;
using PasswordManager.Services;
using Microsoft.Maui.Essentials;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;
using Microsoft.Maui.Controls.Hosting;
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
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(new Views.RegistrationPage(), true);
        }

        private async void Login()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            if (await LoginService.Login(Email, Password))
            {
                var t = (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.NavigationStack;
                (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.PasswordsListPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
                await Task.Delay(100);
                await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
            }
            else
            {
                PopupService.ShowError("Error", "Invalid login. Please try again.");
            }
        }

    }
}
