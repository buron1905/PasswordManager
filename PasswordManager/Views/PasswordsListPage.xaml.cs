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
    public partial class PasswordsListPage : ContentPage
    {
        public PasswordsListPage()
        {
            InitializeComponent();
            BindingContext = new PasswordsListViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as PasswordsListViewModel).GetPasswords();
        }
    }
}
