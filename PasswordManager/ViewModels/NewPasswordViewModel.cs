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
using Command = MvvmHelpers.Commands.Command;
using MvvmHelpers;

namespace PasswordManager.ViewModels
{
    public class NewPasswordViewModel : BaseViewModel
    {
        public ObservableCollection<Password> PasswordsList { get; set; }

        public NewPasswordViewModel()
        {
            SaveCommand = new Command(Save);
        }

        public ICommand SaveCommand { get; }

        string _passwordName = "";
        public string PasswordName
        {
            get => _passwordName;
            set => SetProperty(ref _passwordName, value);
        }

        string _userName = "";
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        string _description = "";
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private async void Save()
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
