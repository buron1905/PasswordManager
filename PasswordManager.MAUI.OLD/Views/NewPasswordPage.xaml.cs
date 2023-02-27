using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class NewPasswordPage : ContentPage
    {
        public NewPasswordViewModel ViewModel { get; set; }
        public NewPasswordPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as NewPasswordViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.Current.Navigating += ViewModel.Current_Navigating;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Shell.Current.Navigating -= ViewModel.Current_Navigating;
        }
    }
}
