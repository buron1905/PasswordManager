using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using System.Collections.ObjectModel;
using MAUIModelsLib;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Microsoft.Maui.Controls;
using Command = MvvmHelpers.Commands.Command;
using System.Threading;
using PasswordManager.Views;

namespace PasswordManager.ViewModels
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
            Title = "List of passwords";

            AllPasswords = new List<Password>();
            FilteredPasswords = new ObservableRangeCollection<Password>();

            LogoutCommand = new AsyncCommand(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            NewPasswordCommand = new AsyncCommand(NewPassword);
            DetailCommand = new AsyncCommand<Password>(Detail);
            DeleteCommand = new AsyncCommand<Password>(Delete);

            PerformSearchCommand = new MvvmHelpers.Commands.Command<string>(PerformSearch);
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

            var filteredList = AllPasswords.Where(x => x.PasswordName.ToLowerInvariant().StartsWith(searchText)).ToList();
            if(filteredList.Count() == 0)
                filteredList = AllPasswords.Where(x => x.PasswordName.ToLowerInvariant().Contains(searchText)).ToList();

            FilteredPasswords.Clear();
            FilteredPasswords.AddRange(filteredList);
        }
    }
}
