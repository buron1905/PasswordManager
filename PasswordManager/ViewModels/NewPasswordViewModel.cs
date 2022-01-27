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
    public class NewPasswordViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public NewPasswordViewModel()
        {
            Create = new Command(OnCreate);
        }

        string _userName = "";
        public string UserName
        {
            get => _userName;
            set
            {
                if (value == _userName)
                    return;
                _userName = value;
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


        public ICommand Create { get; }

        private async void OnCreate()
        {
            if (UserName == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields must not be empty");
                return;
            }

            Password newPassword = new Password();
            newPassword.PasswordText = Password;
            newPassword.UserName = UserName;
            //await DatabaseService.AddPassword(newPassword);

            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync();
        }
    }
}
