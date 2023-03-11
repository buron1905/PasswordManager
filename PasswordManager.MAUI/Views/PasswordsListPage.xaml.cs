using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class PasswordsListPage : ContentPage
    {
        public PasswordsListViewModel ViewModel { get; set; }

        public PasswordsListPage(PasswordsListViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await ViewModel.RefreshPasswords();
            await ViewModel.RefreshCommand.ExecuteAsync(null);
        }
    }
}
