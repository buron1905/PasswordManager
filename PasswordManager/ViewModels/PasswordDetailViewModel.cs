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
    public class PasswordDetailViewModel : BaseViewModel
    {
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        Password _password;
        public Password Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _passwordVisible = false;
        string _toggleVisibilityText = "Show password";
        public string ToggleVisibilityText
        {
            get => _toggleVisibilityText;
            set => SetProperty(ref _toggleVisibilityText, value);
        }

        public PasswordDetailViewModel()
        {
            Title = "Password detail";

            EditCommand = new AsyncCommand(Edit);
            DeleteCommand = new AsyncCommand(Delete);
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }

        private void TogglePasswordVisibility()
        {

        }

        private async Task Edit()
        {
            Views.EditPasswordPage editPasswordPage = new Views.EditPasswordPage(Password);
            await (Application.Current.MainPage as NavigationPage).Navigation.PushAsync(editPasswordPage);
        }

        private async Task Delete()
        {
            if (Password == null)
                return;

            if (await PopupService.ShowYesNo($"{Password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                await DatabaseService.RemovePassword(Password.Id);
                await (Application.Current.MainPage as NavigationPage).Navigation.PopAsync(true);
            }
        }
    }
}
