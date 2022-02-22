using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MAUIModelsLib;
using PasswordManager.ViewModels;

namespace PasswordManager.Views
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
        }
    }
}
