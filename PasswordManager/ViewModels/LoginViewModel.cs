using Microsoft.Maui.Controls;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public LoginViewModel()
        {
            Login = new Command(OnLogin);
            ForgotPassword = new Command(OnForgotPassword);
            Registration = new Command(OnRegistration);
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


        public ICommand Login { get; }
        public ICommand ForgotPassword { get; }
        public ICommand Registration { get; }

        private void OnForgotPassword()
        {

        }

        private void OnRegistration()
        {

        }

        private void OnLogin()
        {
            ActiveUserService.Instance.Password = Password;


            byte[] arr = EncryptionService.EncryptStringToBytes_Aes(Email, ActiveUserService.Instance.Password);
            string emailEncrypted = ParserService.ByteArrayToString(arr);
            Console.WriteLine(emailEncrypted);

            string decrypted = EncryptionService.DecryptStringFromBytes_Aes(arr, Password);
            Console.WriteLine(decrypted);
        }
    }
}
