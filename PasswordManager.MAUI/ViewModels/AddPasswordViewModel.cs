using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Data;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordOriginal), $"{nameof(PasswordDTO)}Duplicate")]
    public partial class AddPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

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

        private readonly IMauiPasswordService _passwordService;

        #endregion

        public AddPasswordViewModel(IMauiPasswordService passwordService)
        {
            Title = "Add password";
            _passwordService = passwordService;
        }

        #region Commands

        [RelayCommand]
        async Task SwitchFavorite()
        {
            Favorite = !Favorite;
            if (Favorite)
                await AlertService.ShowToast("Added to favorites");
            else
                await AlertService.ShowToast("Removed from favorites");
        }

        [RelayCommand]
        async Task Save()
        {
            var model = GetModelFromProperties();

            if (!ValidationHelper.ValidateForm(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            var userGuid = ActiveUserService.Instance.ActiveUser.Id;

            var encryptedModel = await _passwordService.EncryptPasswordAsync(model, ActiveUserService.Instance.CipherKey);
            await _passwordService.CreateAsync(userGuid, encryptedModel);

            PasswordOriginal = model;
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");

            // to clean up field, otherwise they will stay filled
            SetProperties(new PasswordDTO());
            PasswordOriginal = new PasswordDTO();

            IsBusy = false;
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");
            SetProperties(new PasswordDTO());
            PasswordOriginal = new PasswordDTO();
        }

        #endregion

        #region Methods

        public async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            var deferral = e.GetDeferral();

            if (!PropertiesAreSameAsInOriginalPassword())
            {
                if (!await AlertService.ShowYesNo("You have unsaved changes!", "Unsaved changes will be lost. Do you still want to leave?"))
                    e.Cancel();
            }

            deferral.Complete();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.Keys.Contains($"{nameof(PasswordDTO)}Duplicate") && query[$"{nameof(PasswordDTO)}Duplicate"] is PasswordDTO duplicate)
            {
                SetProperties(duplicate);
            }
        }

        PasswordDTO GetModelFromProperties()
        {
            return new PasswordDTO()
            {
                Id = PasswordOriginal.Id,
                IDT = PasswordOriginal.IDT,
                UDT = PasswordOriginal.UDT,
                UDTLocal = PasswordOriginal.UDTLocal,
                DDT = PasswordOriginal.DDT,
                Deleted = PasswordOriginal.Deleted,
                PasswordName = PasswordName,
                UserName = UserName,
                PasswordDecrypted = Password,
                PasswordEncrypted = PasswordOriginal.PasswordEncrypted,
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
            Favorite = password.Favorite;
        }

        void CleanModelEntityInformation(ref PasswordDTO model)
        {
            model.Id = Guid.Empty;
            model.IDT = default;
            model.UDT = default;
            model.UDTLocal = default;
            model.DDT = default;
            model.Deleted = false;
        }

        #endregion
    }
}
