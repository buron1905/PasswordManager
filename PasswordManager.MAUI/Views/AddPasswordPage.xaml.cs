using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class AddPasswordPage : ContentPage
{
    public AddPasswordViewModel ViewModel { get; set; }

    public AddPasswordPage(AddPasswordViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Shell.Current.Navigating += ViewModel.Current_Navigating;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.Current.Navigating -= ViewModel.Current_Navigating;
    }
}