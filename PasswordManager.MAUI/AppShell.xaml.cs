﻿using PasswordManager.MAUI.Views;

namespace PasswordManager.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage),
                    typeof(LoginPage));

            Routing.RegisterRoute(nameof(RegistrationPage),
                    typeof(RegistrationPage));

            Routing.RegisterRoute(nameof(PasswordsListPage),
                    typeof(PasswordsListPage));

        }
    }
}