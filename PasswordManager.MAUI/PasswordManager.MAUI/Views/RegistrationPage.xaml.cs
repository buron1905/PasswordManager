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
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationViewModel ViewModel { get; set; }

        public RegistrationPage()
        {
            InitializeComponent();
			ViewModel = BindingContext as RegistrationViewModel;
        }
    }
}
