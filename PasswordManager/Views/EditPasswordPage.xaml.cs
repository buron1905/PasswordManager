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
        public EditPasswordPage(Password password)
        {
            InitializeComponent();
            ViewModel = (BindingContext as EditPasswordViewModel);
            ViewModel.Password = password;//.DeepCopy();
        }
    }
}
