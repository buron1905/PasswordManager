using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
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

        #endregion

        public PasswordsListViewModel()
        {
            Title = "Passwords";
        }

        #region Commands

        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            await RefreshPasswords();
            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task GoToNewPassword()
        {
            //await Shell.Current.GoToAsync($"{nameof(NewPasswordPage)}");
        }

        [RelayCommand]
        async Task Delete(PasswordDTO password)
        {
            if (password is null) return;

            if (await PopupService.ShowYesNo($"{password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                //await DatabaseService.RemovePassword(password.Id);
                AllPasswords.Remove(password);
                FilteredPasswords.Remove(password);
            }
        }

        [RelayCommand]
        async Task GoToEdit(PasswordDTO password)
        {
            if (password is null) return;

            //await Shell.Current.GoToAsync(nameof(PasswordEditPage), true, new Dictionary<string, object>
            //{
            //    { "password", password }
            //});
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
            AllPasswords.Clear();
            AllPasswords = new List<PasswordDTO>
            {
                new PasswordDTO
                {
                    PasswordName = "Password 1",
                    UserName = "Username 1"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                },
                new PasswordDTO
                {
                    PasswordName = "Password 2",
                    UserName = "Username 2"
                }
            };
            //AllPasswords = await DatabaseService.GetUserPasswords(ActiveUserService.Instance.User.Id);
            PerformSearch();
        }

        #endregion
    }
}
