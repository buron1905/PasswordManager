using PasswordManager.MAUI.Views;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        //public LoadingViewModel(IAuthService authService)
        public LoadingViewModel()
        {
            //_authService = authService;
            CheckAuthentication();
        }


        async void CheckAuthentication()
        {
            var token = await SecureStorage.GetAsync("token");

            if (string.IsNullOrWhiteSpace(token))
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}", true);
            else
            {
                //if (_authService.TokenIsValid(token))
                //{
                //    // TODO: Login
                //    //if (await _authService.LoginAsync(token))
                //    //    await Shell.Current.GoToAsync($"//{nameof(PasswordsListPage)}");
                //    //else
                //    //    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                //}

            }
        }
    }
}
