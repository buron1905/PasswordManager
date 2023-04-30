using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using Services.Abstraction.Data;
using System.Collections.ObjectModel;

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

        private readonly IMauiPasswordService _passwordService;
        private readonly IMauiSyncService _syncService;

        #endregion

        public PasswordsListViewModel(IMauiPasswordService passwordService, IMauiSyncService syncService)
        {
            Title = "Passwords";
            _passwordService = passwordService;
            _syncService = syncService;
        }

        #region Commands

        [RelayCommand]
        async Task Refresh() // parameter for some reason disables refresh, therefore SyncAndRefresh
        {
            IsBusy = true;

            await RefreshPasswords();

            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task SyncAndRefresh()
        {
            IsBusy = true;

            await _syncService.DoSync();
            await RefreshPasswords();

            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task GoToNewPassword()
        {
            await Shell.Current.GoToAsync($"//{nameof(AddPasswordPage)}", true);
        }

        [RelayCommand]
        async Task Delete(PasswordDTO password)
        {
            if (password is null) return;

            if (await AlertService.ShowYesNo($"{password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                var userGuid = ActiveUserService.Instance.ActiveUser.Id;
                await _passwordService.DeleteAsync(userGuid, password);
                AllPasswords.Remove(password);
                FilteredPasswords.Remove(password);
                await _syncService.DoSync();
            }
        }

        [RelayCommand]
        async Task GoToEdit(PasswordDTO password)
        {
            if (password is null) return;

            await Shell.Current.GoToAsync($"//{nameof(EditPasswordPage)}", true, new Dictionary<string, object>
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
                || (x.UserName?.Trim().ToLowerInvariant().Contains(searchText) ?? false) // false because username can be null
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

            var passwords = (await _passwordService.GetAllByUserIdAsync(userId)).Where(x => !x.Deleted).ToList() ?? new List<PasswordDTO>();
            var decryptedPasswords = new List<PasswordDTO>();

            foreach (var password in passwords)
            {
                decryptedPasswords.Add(await _passwordService.DecryptPasswordAsync(password, ActiveUserService.Instance.CipherKey, true));
            }

            AllPasswords = decryptedPasswords;

            PerformSearch();
        }

        #endregion
    }
}
