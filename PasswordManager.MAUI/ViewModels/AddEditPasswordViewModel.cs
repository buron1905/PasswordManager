using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordOriginal), nameof(PasswordDTO))]
    public partial class AddEditPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

        [ObservableProperty]
        PasswordDTO _passwordOriginal = new();

        [ObservableProperty]
        bool _isRefreshing;

        [ObservableProperty]
        string _passwordName;

        [ObservableProperty]
        string _userName;

        [ObservableProperty]
        string _password;

        [ObservableProperty]
        string _description;

        #endregion

        public AddEditPasswordViewModel()
        {
            Title = "New password";
        }

        #region Commands

        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;

            //int.TryParse(PasswordId, out var parsedId);
            //PasswordOriginal = await DatabaseService.GetPassword(parsedId);

            //PasswordName = PasswordOriginal.PasswordName;
            //UserName = PasswordOriginal.UserName;
            //Password = PasswordOriginal.PasswordDecrypted;
            //Description = PasswordOriginal.Notes;

            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task Save()
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
            //await DatabaseService.UpdatePassword(PasswordOriginal);
            await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
        }

        [RelayCommand]
        async Task Cancel()
        {
            //await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}/{nameof(PasswordDetailPage)}?PasswordId={PasswordId}");
        }

        #endregion

        #region Methods

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query[nameof(PasswordDTO).ToString()] is PasswordDTO)
                Title = "Edit password";
        }

        #endregion
    }
}
