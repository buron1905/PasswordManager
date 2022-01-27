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

namespace PasswordManager.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public LoginViewModel()
        {
            Login = new Command(OnLogin);
            Registration = new Command(OnRegistration);
        }

        string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                if (value == _email)
                    return;
                _email = value;
                OnPropertyChanged();
            }
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (value == _password)
                    return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand Login { get; }
        public ICommand Registration { get; }

        private async void OnRegistration()
        {
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(new Views.RegistrationPage());
        }

        private async void OnLogin()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            if (await LoginService.Login(Email, Password))
            {
                await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(new Views.PasswordsListPage());

                //(Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.PasswordsListPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
                //await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync();
            }
            else
            {
                PopupService.ShowError("Error", "Invalid login. Please try again.");
            }
        }
    }
}
