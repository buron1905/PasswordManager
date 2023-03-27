using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class LoginTfaPage : ContentPage
{
    public LoginTfaPage(LoginTfaViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}