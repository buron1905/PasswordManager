using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MAUIModelsLib;
using PasswordManager.ViewModels;

namespace PasswordManager.Views
{
    public partial class EditPasswordPage : ContentPage
    {
        public EditPasswordPage(Password password)
        {
            InitializeComponent();
            (BindingContext as PasswordDetailViewModel).Password = password;
        }
    }
}