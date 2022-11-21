using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Models;
using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class PasswordDetailPage : ContentPage
    {
        public PasswordDetailViewModel ViewModel { get; set; }
        public PasswordDetailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as PasswordDetailViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadPassword();
        }
    }
}
