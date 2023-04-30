using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Data;
using Services.Cryptography;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordOriginal), nameof(PasswordDTO))]
    public partial class EditPasswordViewModel : BaseViewModel, IQueryAttributable
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

        public EditPasswordViewModel(IMauiPasswordService passwordService)
        {
            Title = "Edit password";
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
        async Task Delete()
        {
            if (await AlertService.ShowYesNo($"Delete: {PasswordName}", $"Are you sure you want to delete this password?"))
            {
                var userGuid = ActiveUserService.Instance.ActiveUser.Id;
                await _passwordService.DeleteAsync(userGuid, PasswordOriginal);
                await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");
                await AlertService.ShowToast("Deleted");
            }
        }

        [RelayCommand]
        async Task Save()
        {
            var model = GetModelFromProperties();

            if (!ValidationHelper.ValidateForm(model, Shell.Current.CurrentPage))
                return;

            IsBusy = true;

            var userGuid = ActiveUserService.Instance.ActiveUser.Id;

            model.Id = PasswordOriginal.Id;
            model.UDTLocal = DateTime.UtcNow; // otherwise synced password will not have updated UDT
            var encryptedModel = await _passwordService.EncryptPasswordAsync(model, ActiveUserService.Instance.CipherKey);
            await _passwordService.UpdateAsync(userGuid, encryptedModel);

            PasswordOriginal = model;
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");

            // to clean up field, otherwise they will stay filled
            SetProperties(new PasswordDTO());
            PasswordOriginal = new PasswordDTO();

            IsBusy = false;
        }

        [RelayCommand]
        async Task Duplicate()
        {
            var model = GetModelFromProperties();

            IsBusy = true;

            CleanModelEntityInformation(ref model);

            await Shell.Current.GoToAsync($"//{nameof(AddPasswordPage)}", true, new Dictionary<string, object>
            {
                { $"{nameof(PasswordDTO)}Duplicate", model }
            });

            SetProperties(new PasswordDTO());
            PasswordOriginal = new PasswordDTO();

            IsBusy = false;
        }

        [RelayCommand]
        async Task Cancel()
        {
            IsBusy = true;
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");
            SetProperties(new PasswordDTO());
            PasswordOriginal = new PasswordDTO();
            IsBusy = false;
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
            if (query.Keys.Contains(nameof(PasswordDTO).ToString()) && query[nameof(PasswordDTO).ToString()] is PasswordDTO password)
            {
                password.PasswordDecrypted = await EncryptionService.DecryptUsingAES(password.PasswordEncrypted, ActiveUserService.Instance.CipherKey);
                password.URL = await EncryptionService.DecryptUsingAES(password.URL, ActiveUserService.Instance.CipherKey);
                password.Notes = await EncryptionService.DecryptUsingAES(password.Notes, ActiveUserService.Instance.CipherKey);
                SetProperties(password);
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
