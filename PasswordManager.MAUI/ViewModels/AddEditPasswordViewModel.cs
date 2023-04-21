using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Data;

namespace PasswordManager.MAUI.ViewModels
{
    [QueryProperty(nameof(PasswordOriginal), nameof(PasswordDTO))]
    [QueryProperty(nameof(PasswordOriginal), $"{nameof(PasswordDTO)}Duplicate")]
    public partial class AddEditPasswordViewModel : BaseViewModel, IQueryAttributable
    {
        #region Properties

        [ObservableProperty]
        bool _isRefreshing;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanRefresh))]
        [NotifyPropertyChangedFor(nameof(IsNotNew))]
        bool _isNew;

        public bool CanRefresh => !IsNew;

        public bool IsNotNew => !IsNew;

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

        private readonly IDataServiceWrapper _dataServiceWrapper;

        #endregion

        public AddEditPasswordViewModel(IDataServiceWrapper dataServiceWrapper)
        {
            Title = "Add password";
            IsNew = true;
            _dataServiceWrapper = dataServiceWrapper;
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
                if (!await AlertService.ShowYesNo("Refreshing will discard your changes!", "Unsaved changes will be lost. Do you still want to leave?"))
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
        void RefreshToolbar()
        {
            var page = Shell.Current.CurrentPage;
            page.ToolbarItems.Clear();

            var toolbarSave = new ToolbarItem()
            {
                Text = "Save",
                Command = SaveCommand,
                Order = ToolbarItemOrder.Secondary
            };

            var toolbarDuplicate = new ToolbarItem()
            {
                Text = "Duplicate",
                Command = DuplicateCommand,
                Order = ToolbarItemOrder.Secondary
            };

            var toolbarDelete = new ToolbarItem()
            {
                Text = "Delete",
                Command = DeleteCommand,
                Order = ToolbarItemOrder.Secondary
            };

            if (IsNew)
            {
                page.ToolbarItems.Add(toolbarSave);

                PasswordOriginal = new();
                return;
            }

            page.ToolbarItems.Add(toolbarSave);
            page.ToolbarItems.Add(toolbarDuplicate);
            page.ToolbarItems.Add(toolbarDelete);
        }

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
                await _dataServiceWrapper.PasswordService.DeleteAsync(userGuid, PasswordOriginal.Id);
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
            if (IsNew)
                await _dataServiceWrapper.PasswordService.CreateAsync(userGuid, model);
            else
            {
                model.Id = PasswordOriginal.Id;
                await _dataServiceWrapper.PasswordService.UpdateAsync(userGuid, model);
            }

            PasswordOriginal = model;
            await Shell.Current.GoToAsync($"///{nameof(PasswordsListPage)}");

            IsBusy = false;
        }

        [RelayCommand]
        async Task Duplicate()
        {
            var model = GetModelFromProperties();

            IsBusy = true;

            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
            await Shell.Current.GoToAsync($"//Home/{nameof(AddEditPasswordPage)}", true, new Dictionary<string, object>
            {
                { $"{nameof(PasswordDTO)}Duplicate", model }
            });

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
                if (!await AlertService.ShowYesNo("You have unsaved changes!", "Unsaved changes will be lost. Do you still want to leave?"))
                    e.Cancel();
            }

            deferral.Complete();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.Keys.Contains(nameof(PasswordDTO).ToString()) && query[nameof(PasswordDTO).ToString()] is PasswordDTO password)
            {
                SetProperties(password);

                Title = "Edit password";
                IsNew = false;
            }
            else if (query.Keys.Contains($"{nameof(PasswordDTO)}Duplicate") && query[$"{nameof(PasswordDTO)}Duplicate"] is PasswordDTO duplicate)
            {
                SetProperties(duplicate);

                Title = "Add password";
                IsNew = true;
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
