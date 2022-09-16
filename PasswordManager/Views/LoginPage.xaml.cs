using Microsoft.Maui.Controls;
using PasswordManager.ViewModels;
using System;

namespace PasswordManager.Views
{
	public partial class LoginPage : ContentPage
	{
        public LoginViewModel ViewModel { get; set; }

		public LoginPage()
		{
			InitializeComponent();
			ViewModel = BindingContext as LoginViewModel;
        }
	}
}
