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
    public partial class GeneratePasswordPage : ContentPage
    {
        public GeneratePasswordViewModel ViewModel { get; set; }

        public GeneratePasswordPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as GeneratePasswordViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.GenerateNew();
        }
    }
}
