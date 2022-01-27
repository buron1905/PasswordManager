using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;

namespace PasswordManager.ViewModels
{
    public class RegistrationViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public RegistrationViewModel()
        {
            Register = new Command(OnRegister);
            Login = new Command(OnLogin);
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


        public ICommand Register { get; }
        public ICommand Login { get; }

        private async void OnLogin()
        {
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync();
        }

        private async void OnRegister()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields must not be empty");
                return;
            }

            User newUser = new User();
            newUser.Email = Email;
            newUser.PasswordHASH = HashingService.HashSHA512ToString(Password);
            await DatabaseService.AddUser(newUser);

            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync();
            Microsoft.Maui.Controls.Application.Current.MainPage = new NavigationPage(new Views.PasswordsPage());
        }
    }
}
