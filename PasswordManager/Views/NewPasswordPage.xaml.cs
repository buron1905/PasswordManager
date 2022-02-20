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
    public partial class NewPasswordPage : ContentPage
    {
        public NewPasswordViewModel ViewModel { get; set; }
        public NewPasswordPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as NewPasswordViewModel;
        }
    }
}
