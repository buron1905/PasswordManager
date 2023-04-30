using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views;

public partial class EditPasswordPage : ContentPage
{
    public EditPasswordViewModel ViewModel { get; set; }

    public EditPasswordPage(EditPasswordViewModel viewModel)
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