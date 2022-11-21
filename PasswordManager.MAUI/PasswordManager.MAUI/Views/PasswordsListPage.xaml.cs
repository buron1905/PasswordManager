using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.MAUI.ViewModels;
using Models;

namespace PasswordManager.MAUI.Views
{
    public partial class PasswordsListPage : ContentPage
    {
        public PasswordsListViewModel ViewModel { get; set; }

        public PasswordsListPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as PasswordsListViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.RefreshPasswords();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.PerformSearchCommand.Execute(e.NewTextValue);
        }
    }
}
