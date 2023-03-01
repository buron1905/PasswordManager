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
            ViewModel.Page = this;
        }
    }
}
