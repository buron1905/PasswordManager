﻿using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Services;
using PasswordManager.MAUI.Views;
using PasswordManager.MAUI.Views.Controls;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        #region Properties
        #endregion

        public AppShellViewModel()
        {
            Title = "Password Manager";
        }

        #region Commands

        [RelayCommand]
        async void Logout()
        {
            ActiveUserService.Instance.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        #endregion

        #region Methods

        public static void FlyoutHeaderRefresh()
        {
            Shell.Current.FlyoutHeader = new FlyoutHeader();
        }

        #endregion
    }
}
