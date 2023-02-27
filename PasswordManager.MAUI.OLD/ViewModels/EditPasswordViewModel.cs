using System.Windows.Input;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordId), nameof(PasswordId))]
    public class EditPasswordViewModel : BaseViewModel
    {
        public string PasswordId { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditPasswordViewModel()
        {
            Title = "Edit password";

            SaveCommand = new AsyncCommand(Save);
            CancelCommand = new AsyncCommand(Cancel);
        }

        Password _passwordOriginal;
        public Password PasswordOriginal
        {
            get => _passwordOriginal;
            set => SetProperty(ref _passwordOriginal, value);
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

            if (UserName != PasswordOriginal.UserName || Password != PasswordOriginal.PasswordDecrypted || PasswordName != PasswordOriginal.PasswordName)
            {
                if (!await PopupService.ShowYesNo("Leave unsaved changes?", "Unsaved changes will be lost. Do you still want to leave?"))
                    e.Cancel();
            }

            deferral.Complete();
        }

        public async Task LoadPassword()
        {
            int.TryParse(PasswordId, out var parsedId);
            PasswordOriginal = await DatabaseService.GetPassword(parsedId);

            PasswordName = PasswordOriginal.PasswordName;
            UserName = PasswordOriginal.UserName;
            Password = PasswordOriginal.PasswordDecrypted;
            Description = PasswordOriginal.Notes;
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordName))
            {
                await PopupService.ShowError("Error", "Fields cannot be empty");
                return;
            }

            PasswordOriginal.PasswordName = PasswordName;
            PasswordOriginal.UserName = UserName;
            PasswordOriginal.PasswordDecrypted = Password;
            PasswordOriginal.Notes = Description;
            await DatabaseService.UpdatePassword(PasswordOriginal);
            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}/{nameof(PasswordDetailPage)}?PasswordId={PasswordId}");
        }
    }
}
