using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class PasswordsListPage : ContentPage
    {
        public PasswordsListViewModel ViewModel { get; set; }

        public PasswordsListPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as PasswordsListViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.RefreshPasswords();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.PerformSearchCommand.Execute(e.NewTextValue);
        }
    }
}
