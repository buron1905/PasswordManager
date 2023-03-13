using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordOriginal), nameof(PasswordDTO))]
    public partial class AddEditPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

        [ObservableProperty]
        bool _isRefreshing;

        [ObservableProperty]
        bool _canRefresh;

        [ObservableProperty]
        PasswordDTO _passwordOriginal = new();

        [ObservableProperty]
        string _passwordName;

        [ObservableProperty]
        string _userName;

        [ObservableProperty]
        string _password;

        [ObservableProperty]
        string _URL;

        [ObservableProperty]
        string _notes;

        [ObservableProperty]
        bool _favorite;

        #endregion

        public AddEditPasswordViewModel()
        {
            Title = "New password";
            CanRefresh = false;
        }

        #region Commands

        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;

            //int.TryParse(PasswordId, out var parsedId);
            //PasswordOriginal = await DatabaseService.GetPassword(Password.Id);

            //PasswordName = PasswordOriginal.PasswordName;
            //UserName = PasswordOriginal.UserName;
            //Password = PasswordOriginal.PasswordDecrypted;
            //Description = PasswordOriginal.Notes;
            //TODO get new data

            if (!PropertiesAreSameAsInOriginalPassword())
            {
                if (!await PopupService.ShowYesNo("Refreshing will discard your changes!", "Unsaved changes will be lost. Do you still want to leave?"))
                {
                    IsBusy = false;
                    IsRefreshing = false;
                    return;
                }
            }

            SetProperties(PasswordOriginal);

            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task Save()
        {
            var model = GetModelFromProperties();

            if (!ValidationHelper.IsFormValid(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            //await DatabaseService.UpdatePassword(PasswordOriginal);
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");

            IsBusy = false;
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");
        }

        #endregion

        #region Methods

        public async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var deferral = e.GetDeferral();

            if (!PropertiesAreSameAsInOriginalPassword())
            {
                if (!await PopupService.ShowYesNo("You have unsaved changes!", "Unsaved changes will be lost. Do you still want to leave?"))
                    e.Cancel();
            }

            deferral.Complete();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query[nameof(PasswordDTO).ToString()] is PasswordDTO password)
            {
                Title = "Edit password";
                CanRefresh = true;
                SetProperties(password);
            }
        }

        PasswordDTO GetModelFromProperties()
        {
            return new PasswordDTO()
            {
                PasswordName = PasswordName,
                UserName = UserName,
                PasswordDecrypted = Password,
                URL = URL,
                Notes = Notes,
                Favorite = Favorite
            };
        }

        bool PropertiesAreSameAsInOriginalPassword()
        {
            return PasswordName == PasswordOriginal.PasswordName
                && UserName == PasswordOriginal.UserName
                && Password == PasswordOriginal.PasswordDecrypted
                && URL == PasswordOriginal.URL
                && Notes == PasswordOriginal.Notes
                && Favorite == PasswordOriginal.Favorite;
        }

        void SetProperties(PasswordDTO password)
        {
            PasswordName = password.PasswordName;
            UserName = password.UserName;
            Password = password.PasswordDecrypted;
            URL = password.URL;
            Notes = password.Notes;
            Favorite = password.Favorite; ;
        }

        #endregion
    }
}
