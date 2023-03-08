using Models.DTOs;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace PasswordManager.MAUI.ViewModels
{
    public class PasswordsListViewModel : BaseViewModel
    {
        public List<PasswordDTO> AllPasswords { get; set; }
        public ObservableRangeCollection<PasswordDTO> FilteredPasswords { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NewPasswordCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PerformSearchCommand { get; }

        public PasswordsListViewModel()
        {
            Title = "Passwords";

            AllPasswords = new List<PasswordDTO>();
            FilteredPasswords = new ObservableRangeCollection<PasswordDTO>();

            LogoutCommand = new AsyncCommand(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            NewPasswordCommand = new AsyncCommand(NewPassword);
            EditCommand = new AsyncCommand<PasswordDTO>(Edit);
            DeleteCommand = new AsyncCommand<PasswordDTO>(Delete);

            PerformSearchCommand = new Command(PerformSearch);
            //PerformSearchCommand = new MvvmHelpers.Commands.Command<string>(PerformSearch);
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
            Thread.Sleep(1000);
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

        private async Task NewPassword()
        {
            //await Shell.Current.GoToAsync($"{nameof(NewPasswordPage)}");
        }

        private async Task Delete(PasswordDTO password)
        {
            if (password is null)
                return;

            if (await PopupService.ShowYesNo($"{password.PasswordName}", $"Are you sure you want to delete this password?"))
            {
                //await DatabaseService.RemovePassword(password.Id);
                AllPasswords.Remove(password);
                FilteredPasswords.Remove(password);
            }
        }

        private async Task Edit(PasswordDTO password)
        {
            //await Shell.Current.GoToAsync($"{nameof(PasswordDetailPage)}?PasswordId={password.Id}");
        }

        private void PerformSearch()
        {
            var searchText = SearchText ?? string.Empty;
            searchText = searchText.Trim().ToLowerInvariant();

            var filteredList = AllPasswords.Where(x =>
                x.PasswordName.Trim().ToLowerInvariant().Contains(searchText)
                || x.UserName.Trim().ToLowerInvariant().Contains(searchText)
                ).OrderBy(x => x.PasswordName).ThenBy(x => x.UserName).ToList();

            FilteredPasswords.Clear();
            FilteredPasswords.AddRange(filteredList);

        }
    }
}
