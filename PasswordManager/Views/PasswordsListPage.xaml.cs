using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.ViewModels;
using MAUIModelsLib;

namespace PasswordManager.Views
{
    public partial class PasswordsListPage : ContentPage
    {
        public PasswordsListPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as PasswordsListViewModel).RefreshPasswords();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            (BindingContext as PasswordsListViewModel).PerformSearchCommand.Execute(e.NewTextValue);
        }
    }
}
