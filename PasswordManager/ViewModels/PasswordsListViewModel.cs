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
            NewPassword = new Command(OnNewPassword);
            Update = new Command(OnUpdate);
            Delete = new Command(OnDelete);
            Detail = new Command(OnDetail);

            Passwords = new ObservableCollection<Password>();
            GetPasswords();
        }
        
        public ICommand Logout { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NewPassword { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public ICommand Detail { get; }

        Password _selectedPassword;
        public Password SelectedPassword
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

        bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (value == _isBusy)
                    return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private async void OnLogout()
        {
            ActiveUserService.Instance.Logout();

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.LoginPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await Task.Delay(100);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync(true);
        }

        private void OnRefreshCommand()
        {
            GetPasswords();
            IsBusy = false;
        }

        private async void GetPasswords()
        {
            Passwords.Clear();

            List<Password> passwordsList = await DatabaseService.GetUserPasswords(ActiveUserService.Instance.User.Id);
            passwordsList.ForEach(Passwords.Add);
        }

        private async void OnNewPassword()
        {
            Views.NewPasswordPage newPasswordPage = new Views.NewPasswordPage();
            (newPasswordPage.BindingContext as NewPasswordViewModel).PasswordsList = this.Passwords;
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(newPasswordPage);
        }

        private async void OnUpdate()
        {
            Views.EditPasswordPage editPasswordPage = new Views.EditPasswordPage();
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(editPasswordPage);
        }

        private async void OnDelete()
        {
            //if(await PopupService.ShowYesNo($"{SelectedPassword.PasswordName}", $"Are you sure you want to delete this password?"))
            //{
            //    int id = SelectedPassword.Id;
            //    await DatabaseService.RemovePassword(id);
            //    Passwords.Remove(SelectedPassword);
            //}
        }

        private async void OnDetail()
        {
            PopupService.ShowError("TEST", "Detail");
            Console.WriteLine(Passwords.Count());
            //Views.PasswordDetailPage passwordDetailPage = new Views.PasswordDetailPage(SelectedPassword);
            //await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PushAsync(passwordDetailPage);
        }
    }
}
