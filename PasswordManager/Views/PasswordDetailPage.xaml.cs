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
    public partial class PasswordDetailPage : ContentPage
    {
        public PasswordDetailViewModel ViewModel { get; set; }
        public PasswordDetailPage(Password password)
        {
            InitializeComponent();
            (BindingContext as PasswordDetailViewModel).Password = password;
        }
    }
}
