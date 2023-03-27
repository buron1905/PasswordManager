using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class RegistrationSuccessfulPage : ContentPage
{
    public RegistrationSuccessfulPage(RegistrationSuccessfulViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}