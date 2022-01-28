using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using System.Collections.ObjectModel;
using MAUIModelsLib;

namespace PasswordManager.ViewModels
{
    public class PasswordsListViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;
        public ObservableCollection<Password> Passwords { get; set; }

        public PasswordsListViewModel()
        {
            Logout = new Command(OnLogout);
            RefreshCommand = new Command(OnRefreshCommand);

            Passwords = new ObservableCollection<Password>();
            GetPasswords();
        }
        
        public ICommand Logout { get; }
        public ICommand RefreshCommand { get; }

        string _selectedPassword = "";
        public string SelectedPassword
        {
            get => _selectedPassword;
            set
            {
                if (value == _selectedPassword)
                    return;
                _selectedPassword = value;
                OnPropertyChanged();
            }
        }

        private async void OnLogout()
        {
            ActiveUserService.Instance.Logout();

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.LoginPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync();
        }

        private async void OnRefreshCommand()
        {
            GetPasswords();
            PopupService.ShowError("Refreshed", "Refreshed");
        }

        private async void GetPasswords()
        {
            Passwords = new ObservableCollection<Password>(await DatabaseService.GetUserPasswords(ActiveUserService.Instance.User.Id));
            Passwords.Add(new Password());
        }
    }
}
