using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public class NewPasswordViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;
        public ObservableCollection<Password> PasswordsList { get; set; }

        public NewPasswordViewModel()
        {
            Save = new Command(OnSave);
        }

        public ICommand Save { get; }

        string _passwordName = "";
        public string PasswordName
        {
            get => _passwordName;
            set
            {
                if (value == _passwordName)
                    return;
                _passwordName = value;
                OnPropertyChanged();
            }
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

        string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                if (value == _description)
                    return;
                _description = value;
                OnPropertyChanged();
            }
        }

        private async void OnSave()
        {
            if (Password == "" || PasswordName == "" || UserName == "")
            {
                PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            Password newPassword = new Password()
            {
                UserId = ActiveUserService.Instance.User.Id,
                PasswordName = PasswordName,
                UserName = UserName,
                PasswordText = Password,
                Description = Description
            };

            await DatabaseService.AddPassword(newPassword);
            PasswordsList.Add(newPassword);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync(true);
        }

    }
}
