﻿using System.Windows.Input;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI.ViewModels
{
    public class PasswordsListViewModel : BaseViewModel
    {
        public List<Password> AllPasswords { get; set; }
        public ObservableRangeCollection<Password> FilteredPasswords { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NewPasswordCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DetailCommand { get; }
        public ICommand PerformSearchCommand { get; }

        public PasswordsListViewModel()
        {
            Title = "Vault";

            AllPasswords = new List<Password>();
            FilteredPasswords = new ObservableRangeCollection<Password>();

            LogoutCommand = new AsyncCommand(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            NewPasswordCommand = new AsyncCommand(NewPassword);
            DetailCommand = new AsyncCommand<Password>(Detail);
            DeleteCommand = new AsyncCommand<Password>(Delete);

            PerformSearchCommand = new MvvmHelpers.Commands.Command<string>(PerformSearch);
        }

        bool _noPasswordsLabelIsVisible;
        public bool NoPasswordsLabelIsVisible
        {
            get => _noPasswordsLabelIsVisible;
            set => SetProperty(ref _noPasswordsLabelIsVisible, value);
        }

        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private async Task Logout()
        {
            ActiveUserService.Instance.Logout();

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async Task Refresh()
        {
            IsBusy = true;
            await RefreshPasswords();
            IsBusy = false;
        }

        public async Task RefreshPasswords()
        {
            AllPasswords.Clear();
            AllPasswords = await DatabaseService.GetUserPasswords(ActiveUserService.Instance.User.Id);

            PerformSearch(SearchText);
        }

        private async Task NewPassword()
        {
            await Shell.Current.GoToAsync($"{nameof(NewPasswordPage)}");
        }

        private async Task Delete(Password password)
        {
            if (password == null)
                return;

            if (await PopupService.ShowYesNo($"{password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                await DatabaseService.RemovePassword(password.Id);
                AllPasswords.Remove(password);
                FilteredPasswords.Remove(password);
            }
        }

        private async Task Detail(Password password)
        {
            await Shell.Current.GoToAsync($"{nameof(PasswordDetailPage)}?PasswordId={password.Id}");
        }

        private void PerformSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                searchText = string.Empty;

            searchText = searchText.ToLowerInvariant();

            var filteredList = AllPasswords.Where(x => x.PasswordName.ToLowerInvariant().Contains(searchText)).OrderBy(x => x.PasswordName).ThenBy(x => x.UserName).ToList();

            FilteredPasswords.Clear();
            FilteredPasswords.AddRange(filteredList);

            NoPasswordsLabelIsVisible = FilteredPasswords.Count == 0;
        }
    }
}