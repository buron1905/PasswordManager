using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;

namespace PasswordManager.ViewModels
{
    public class RegistrationViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public RegistrationViewModel()
        {
            Register = new Command(OnRegister);
            Login = new Command(OnLogin);
        }

        string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                if (value == _email)
                    return;
                _email = value;
                OnPropertyChanged();
            }
        }

        string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (value == _password)
                    return;
                _password = value;
                OnPropertyChanged();
            }
        }


        public ICommand Register { get; }
        public ICommand Login { get; }

        private async void OnLogin()
        {
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync();
        }

        /*
        private async void OnRegister()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields must not be empty");
                return;
            }

            User newUser = new User();
            newUser.Email = Email;
            newUser.PasswordHASH = HashingService.HashSHA512ToString(Password);

            try
            {
                await DatabaseService.AddUser(newUser);
            }
            catch (Exception ex)
            {
                PopupService.ShowError("Error", $"{ex.Message}");
                return;
            }

            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.InsertPageBefore(new Views.PasswordsListPage(), Application.Current.MainPage.Navigation.NavigationStack[0]);
            await (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopToRootAsync();
        }
        */

        private async void OnRegister()
        {
            if (Email == "" || Password == "")
            {
                PopupService.ShowError("Error", "Fields must not be empty");
                return;
            }

            User newUser = new User();
            newUser.Email = Email;
            newUser.PasswordHASH = HashingService.HashSHA512ToString(Password);

            try
            {
                await DatabaseService.AddUser(newUser);
            }
            catch (Exception ex)
            {
                PopupService.ShowError("Error", $"{ex.Message}");
                return;
            }






            var usersTable = await DatabaseService.GetUsersTableAsync();
            var passwordsTable = await DatabaseService.GetPasswordsTableAsync();
            var settingsTable = await DatabaseService.GetSettingsTableAsync();

            Password password = new Password()
            {
                PasswordName = "Name",
                UserName = "Username",
                PasswordText = "Text",
                UserId = newUser.Id
            };

            await DatabaseService.AddPassword(password);


            Settings settings = new Settings()
            {
                UserId = newUser.Id
            };

            await DatabaseService.AddSettings(settings);

            var usersTable2 = await DatabaseService.GetUsersTableAsync();
            var passwordsTable2 = await DatabaseService.GetPasswordsTableAsync();
            var settingsTable2 = await DatabaseService.GetSettingsTableAsync();

            await DatabaseService.RemoveUser(newUser.Id);
            await DatabaseService.RemovePassword(passwordsTable2[0].Id);
            await DatabaseService.RemoveSettings(settingsTable2[0].Id);

            var usersTable3 = await DatabaseService.GetUsersTableAsync();
            var passwordsTable3 = await DatabaseService.GetPasswordsTableAsync();
            var settingsTable3 = await DatabaseService.GetSettingsTableAsync();
        }
    }
}
