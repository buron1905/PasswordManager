using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class TfaSettingsPage : ContentPage
{
    public TfaSettingsViewModel ViewModel { get; set; }

    public TfaSettingsPage(TfaSettingsViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.GetTfaSetupCommand.ExecuteAsync(null);
    }
}