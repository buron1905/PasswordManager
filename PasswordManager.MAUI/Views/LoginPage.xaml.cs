﻿using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}
