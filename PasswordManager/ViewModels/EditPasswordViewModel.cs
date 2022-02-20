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
using MvvmHelpers.Commands;

namespace PasswordManager.ViewModels
{
    public class EditPasswordViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        Password _password;
        public Password Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public EditPasswordViewModel()
        {
            Title = "Edit password";

            DeleteCommand = new AsyncCommand(Delete);
            SaveCommand = new AsyncCommand(Save);
        }

        private async Task Save()
        {
            if (Password.PasswordText == "" || Password.PasswordName == "" || Password.UserName == "")
            {
                PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }
            await DatabaseService.UpdatePassword(Password);
            await (Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
        }

        private async Task Delete()
        {
            if (Password == null)
                return;

            if (await PopupService.ShowYesNo($"{Password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                await DatabaseService.RemovePassword(Password.Id);
                await (Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
            }
        }
    }
}
