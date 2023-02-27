using PasswordManager.MAUI.ViewModels;

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
