using PasswordManager.MAUI.ViewModels;

namespace PasswordManager.MAUI.Views
{
    public partial class GeneratePasswordPage : ContentPage
    {
        public GeneratePasswordViewModel ViewModel { get; set; }

        public GeneratePasswordPage(GeneratePasswordViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    ViewModel.GenerateCommand.Execute(null);
        //}
    }
}
