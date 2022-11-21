using Microsoft.Maui.Controls;
using PasswordManager.MAUI.ViewModels;
using System;

namespace PasswordManager.MAUI.Views
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
