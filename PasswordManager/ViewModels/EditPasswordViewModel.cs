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
using PasswordManager.Views;

namespace PasswordManager.ViewModels
{
    [QueryProperty(nameof(PasswordId), nameof(PasswordId))]
    public class EditPasswordViewModel : BaseViewModel
    {
        public string PasswordId { get; set; }
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

        public async Task LoadPassword()
        {
            int.TryParse(PasswordId, out var parsedId);
            Password = await DatabaseService.GetPassword(parsedId);
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Password.PasswordText) || string.IsNullOrWhiteSpace(Password.PasswordName) || string.IsNullOrWhiteSpace(Password.UserName))
            {
                await PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }
            await DatabaseService.UpdatePassword(Password);
            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
        }

        private async Task Delete()
        {
            if (Password == null)
                return;

            if (await PopupService.ShowYesNo($"{Password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                await DatabaseService.RemovePassword(Password.Id);
                await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
            }
        }
    }
}
