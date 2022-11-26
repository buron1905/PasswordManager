using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class EditPasswordPage : ContentPage
    {
        public EditPasswordViewModel ViewModel { get; set; }
        public EditPasswordPage()
        {
            InitializeComponent();
            ViewModel = (BindingContext as EditPasswordViewModel);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadPassword();
            Shell.Current.Navigating += ViewModel.Current_Navigating;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Shell.Current.Navigating -= ViewModel.Current_Navigating;
        }
    }
}
