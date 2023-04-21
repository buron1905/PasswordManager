using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class RegistrationPage : ContentPage
    {

        public RegistrationPage(RegistrationViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

    }
}
