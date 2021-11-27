using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            Microsoft.Maui.Controls.Application.Current.MainPage = new Views.LoginPage();
        }

        private void OnRegister()
        {

        }
    }
}
