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

        private void OnLogin()
        {
            (Microsoft.Maui.Controls.Application.Current.MainPage as NavigationPage).Navigation.PopAsync();
        }

        private async void OnRegister()
        {
            if (Email == "" || Password == "")
                return;

            User newUser = new User();
            newUser.Email = Email;
            newUser.PasswordHASH = HashingService.HashSHA512ToString(Password);
            newUser.Id = await DatabaseService.AddUser(newUser);
            //await DatabaseService.AddUser(newUser);
            //newUser.Id = (await DatabaseService.GetUser(newUser.Email)).Id;

            //User newUser2 = new User();
            //newUser2.Email = Email + "2";
            //newUser2.PasswordHASH = HashingService.HashSHA256ToString(Password);
            //newUser2.Id = await DatabaseService.AddUser(newUser2);

            //Password password1 = new Password()
            //{
            //    PasswordName = "test1",
            //    PasswordEncrypted = ParserService.ByteArrayToString(EncryptionService.EncryptStringToBytes_Aes("admin1", Password)),
            //    PasswordDecrypted = "test"
            //};

            //Password password2 = new Password()
            //{
            //    PasswordName = "test2",
            //    UserId = 10051,
            //    PasswordEncrypted = ParserService.ByteArrayToString(EncryptionService.EncryptStringToBytes_Aes("admin1", Password))
            //};


            //await DatabaseService.Add(password1);
            //await DatabaseService.Add(password2);

            List<User> People = await DatabaseService.GetPeopleAsync();
            //List<Password> passwords = await DatabaseService.GetPasswords();

            //newUser.Passwords = new List<Password> { password1, password2 };
            //DatabaseService.UpdateWithChildren(newUser);
            
            //List<User> People2 = await DatabaseService.GetPeopleAsync();

            //User newUserUpdated = DatabaseService.GetWithChildren(newUser.Id);


            //List<User> People = await DatabaseService.GetPeopleAsync();

            //User newUser = new User();
            //newUser.Email = Email;
            //newUser.PasswordHASH = HashingService.HashSHA256ToString(Password);
            //newUser.Id = await DatabaseService.AddUser(newUser);
            //User dbUser = await DatabaseService.GetUser(newUser.Email);

            //List<User> People2 = await DatabaseService.GetPeopleAsync();

            //List<Settings> Set1 = await DatabaseService.GetSettingsTable();

            //Settings settings = new Settings();
            //settings.UserId = newUser.Id;
            //settings.Id = await DatabaseService.Add(settings);
            //Settings dbSettings = await DatabaseService.GetSettings(newUser.Id);

            //List<Settings> Set2 = await DatabaseService.GetSettingsTable();


            //Settings settings2 = new Settings();
            //settings2.UserId = newUser.Id + 50;
            //settings2.SavePassword = false;
            //settings2.Id = await DatabaseService.Add(settings2);
            //Settings dbSettings2 = await DatabaseService.GetSettings(newUser.Id);

            //List<Settings> Set3 = await DatabaseService.GetSettingsTable();

        }
    }
}
