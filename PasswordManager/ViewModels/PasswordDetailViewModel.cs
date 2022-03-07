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
    public class PasswordDetailViewModel : BaseViewModel
    {
        public string PasswordId { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        private bool _passwordVisible = false;

        public PasswordDetailViewModel()
        {
            Title = "Detail";

            EditCommand = new AsyncCommand(Edit);
            DeleteCommand = new AsyncCommand(Delete);
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }

        Password _password;
        public Password Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public async Task LoadPassword()
        {
            int.TryParse(PasswordId, out var parsedId);
            Password = await DatabaseService.GetPassword(parsedId);
        }

        private void TogglePasswordVisibility()
        {

        }

        private async Task Edit()
        {
            await Shell.Current.GoToAsync($"{nameof(EditPasswordPage)}?PasswordId={Password.Id}");
        }

        private async Task Delete()
        {
            if (Password == null)
                return;

            if (await PopupService.ShowYesNo($"{Password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                await DatabaseService.RemovePassword(Password.Id);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
