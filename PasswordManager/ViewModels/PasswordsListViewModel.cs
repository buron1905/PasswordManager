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

namespace PasswordManager.ViewModels
{
    public class PasswordsListViewModel : BaseViewModel
    {
        public List<Password> AllPasswords { get; set; }
        public ObservableRangeCollection<Password> FilteredPasswords { get; set; }

        public PasswordsListViewModel()
        {
            Title = "List of passwords";

            AllPasswords = new List<Password>();
            FilteredPasswords = new ObservableRangeCollection<Password>();

            LogoutCommand = new Command(Logout);
            RefreshCommand = new AsyncCommand(Refresh);
            NewPasswordCommand = new Command(NewPassword);
            DetailCommand = new AsyncCommand<Password>(Detail);
            DeleteCommand = new AsyncCommand<Password>(Delete);
            PerformSearchCommand = new MvvmHelpers.Commands.Command<string>(PerformSearch);
        }

        public ICommand LogoutCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NewPasswordCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DetailCommand { get; }
        public ICommand PerformSearchCommand { get; }

        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private async void Logout()
        {
            ActiveUserService.Instance.Logout();

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.LoginPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await Task.Delay(100);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
        }

        private async Task Refresh()
        {
            IsBusy = true;

            RefreshPasswords();

            IsBusy = false;
        }

        public async void RefreshPasswords()
        {
            AllPasswords.Clear();
            //AllPasswords = new List<Password>() { new Password() { PasswordName = "Test1" }, new Password() { PasswordName = "Test2" } };
            AllPasswords = await DatabaseService.GetUserPasswords(ActiveUserService.Instance.User.Id);

            FilteredPasswords.Clear();
            FilteredPasswords.AddRange(AllPasswords);

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    Task.Delay(100).Wait();
            //    FilteredPasswords.AddRange(AllPasswords);
            //    Task.Delay(100).Wait();
            //});

            //PerformSearch(SearchText);
            //PerformSearchCommand.Execute(SearchText); // refresh for FilteredPasswords
        }

        private async void NewPassword()
        {
            Views.NewPasswordPage newPasswordPage = new Views.NewPasswordPage();
            (newPasswordPage.BindingContext as NewPasswordViewModel).PasswordsList = FilteredPasswords;
            await (Application.Current.MainPage as NavigationPage).Navigation.PushAsync(newPasswordPage);
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
            Views.PasswordDetailPage passwordDetailPage = new Views.PasswordDetailPage(password);
            (passwordDetailPage.BindingContext as PasswordDetailViewModel).PasswordsList = FilteredPasswords;
            await (Application.Current.MainPage as NavigationPage).Navigation.PushAsync(passwordDetailPage);
        }

        private void PerformSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                searchText = string.Empty;

            searchText = searchText.ToLowerInvariant();

            var filteredList = AllPasswords.Where(x => x.PasswordName.ToLowerInvariant().StartsWith(searchText)).ToList();
            if(filteredList.Count() == 0)
            {
                filteredList = AllPasswords.Where(x => x.PasswordName.ToLowerInvariant().Contains(searchText)).ToList();
            }

            //Device.BeginInvokeOnMainThread(() =>
            //{
            FilteredPasswords.Clear();
            FilteredPasswords.AddRange(filteredList);
            //});

            //foreach (var item in AllPasswords)
            //{
            //    if(!filteredList.Contains(item))
            //    {
            //        //Device.BeginInvokeOnMainThread(() => {
            //        //    FilteredPasswords.Remove(item);
            //        //});
            //        //FilteredPasswords.Remove(item);
            //    }
            //    else if(!FilteredPasswords.Contains(item))
            //    {
            //        //Device.BeginInvokeOnMainThread(() => {
            //        //    FilteredPasswords.Add(item);
            //        //});
            //        //FilteredPasswords.Add(item);
            //    }
            //}
        }
    }
}
