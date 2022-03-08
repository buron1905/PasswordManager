using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.ViewModels;

namespace PasswordManager.Views
{
    public partial class GeneratePasswordPage : ContentPage
    {
        public GeneratePasswordViewModel ViewModel { get; set; }

        public GeneratePasswordPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as GeneratePasswordViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.GenerateNew();
        }
    }
}
