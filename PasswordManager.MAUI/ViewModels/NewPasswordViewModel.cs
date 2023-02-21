using System.Windows.Input;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.ViewModels
{
    public class NewPasswordViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public NewPasswordViewModel()
        {
            Title = "New password";

            SaveCommand = new AsyncCommand(Save);
            CancelCommand = new AsyncCommand(Cancel);
        }

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

        public async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var deferral = e.GetDeferral();

            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(PasswordName))
            {
                if (!await PopupService.ShowYesNo("Leave unsaved changes?", "Unsaved changes will be lost. Do you still want to leave?"))
                    e.Cancel();
            }

            deferral.Complete();
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordName))
            {
                await PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            Password newPassword = new Password()
            {
                UserId = ActiveUserService.Instance.User.Id,
                PasswordName = PasswordName,
                UserName = UserName,
                PasswordDecrypted = Password,
                Notes = Description
            };

            await DatabaseService.AddPassword(newPassword);
            Shell.Current.Navigating -= Current_Navigating;
            await Shell.Current.GoToAsync("..");
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
