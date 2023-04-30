using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class TfaSettingsPage : ContentPage
{
    public TfaSettingsPage(TfaSettingsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}