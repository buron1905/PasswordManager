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
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel ViewModel { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as SettingsViewModel;
        }
    }
}
