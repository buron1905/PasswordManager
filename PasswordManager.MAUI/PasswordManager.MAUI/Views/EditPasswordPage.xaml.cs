using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;
using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class EditPasswordPage : ContentPage
    {
        public EditPasswordViewModel ViewModel { get; set; }
        public EditPasswordPage()
        {
            InitializeComponent();
            ViewModel = (BindingContext as EditPasswordViewModel);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadPassword();
            Shell.Current.Navigating += ViewModel.Current_Navigating;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Shell.Current.Navigating -= ViewModel.Current_Navigating;
        }
    }
}
