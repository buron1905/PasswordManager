using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}