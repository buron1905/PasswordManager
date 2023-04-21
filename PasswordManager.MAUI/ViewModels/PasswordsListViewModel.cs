using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Data;
using Services.Cryptography;
using System.Collections.ObjectModel;
using System.Text;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class PasswordsListViewModel : BaseViewModel
    {
        #region Properties

        List<PasswordDTO> AllPasswords { get; set; } = new();
        public ObservableCollection<PasswordDTO> FilteredPasswords { get; } = new();

        [ObservableProperty]
        string _searchText;

        [ObservableProperty]
        bool _isRefreshing;

        private readonly IDataServiceWrapper _dataServiceWrapper;
        private readonly ISyncService _syncService;

        #endregion

        public PasswordsListViewModel(IDataServiceWrapper dataServiceWrapper, ISyncService syncService)
        {
            Title = "Passwords";
            _dataServiceWrapper = dataServiceWrapper;
            _syncService = syncService;
        }

        #region Commands

        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;
            await RefreshPasswords();
            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task GoToNewPassword()
        {
            await Shell.Current.GoToAsync($"//Home/{nameof(AddEditPasswordPage)}", true);
        }

        [RelayCommand]
        async Task Delete(PasswordDTO password)
        {
            if (password is null) return;

            if (await AlertService.ShowYesNo($"{password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                var userGuid = ActiveUserService.Instance.ActiveUser.Id;
                await _dataServiceWrapper.PasswordService.DeleteAsync(userGuid, password.Id);
                AllPasswords.Remove(password);
                FilteredPasswords.Remove(password);
            }
        }

        [RelayCommand]
        async Task GoToEdit(PasswordDTO password)
        {
            if (password is null) return;

            await Shell.Current.GoToAsync(nameof(AddEditPasswordPage), true, new Dictionary<string, object>
            {
                { nameof(PasswordDTO), password }
            });
        }

        [RelayCommand]
        void PerformSearch()
        {
            var searchText = SearchText ?? string.Empty;
            searchText = searchText.Trim().ToLowerInvariant();

            var filteredList = AllPasswords.Where(x =>
                x.PasswordName.Trim().ToLowerInvariant().Contains(searchText)
                || x.UserName.Trim().ToLowerInvariant().Contains(searchText)
                ).OrderBy(x => x.PasswordName).ThenBy(x => x.UserName).ToList();

            FilteredPasswords.Clear();

            foreach (var password in filteredList)
                FilteredPasswords.Add(password);
        }

        #endregion

        #region Methods

        async Task RefreshPasswords()
        {
            var userId = ActiveUserService.Instance.ActiveUser.Id;

            await _syncService.GetLastChangeDateTime(userId);

            var passwords = (await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(userId)).ToList() ?? new List<PasswordDTO>();

            foreach (var password in passwords)
            {
                password.PasswordDecrypted = await EncryptionService.DecryptAsync(Encoding.Unicode.GetBytes(password.PasswordEncrypted ?? string.Empty),
                    ActiveUserService.Instance.CipherKey);
            }

            AllPasswords = passwords;

            PerformSearch();
        }

        #endregion
    }
}
