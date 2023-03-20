using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsViewModel ViewModel { get; set; }

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}