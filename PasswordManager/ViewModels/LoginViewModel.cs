﻿using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ForgotPassword = new Command(OnLogin);
            Registration = new Command(OnLogin);
        }

        string _appName = "Password Manager";
        public string AppName
        { 
            get => _appName;
            set
            {
                if (value == _appName)
                    return;
                _appName = value;
                OnPropertyChanged();
            }
        }


        string _email = "emas";
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

        string _password = "asdf";
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

        private void OnLogin()
        {
            AppName = $"{Email} - {Password}";

            //if (_email != "admin" || _password != "admin")
            //{
            //    //DisplayInvalidLoginPrompt();
            //}
            //else
            //{
            //    AppName += "- success";
            //}
        }
    }
}
