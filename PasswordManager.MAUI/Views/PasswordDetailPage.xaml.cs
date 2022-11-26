using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class PasswordDetailPage : ContentPage
    {
        public PasswordDetailViewModel ViewModel { get; set; }
        public PasswordDetailPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as PasswordDetailViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadPassword();
        }
    }
}
