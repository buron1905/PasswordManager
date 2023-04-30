using CommunityToolkit.Mvvm.Input;
using PasswordManager.MAUI.Services;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class TfaSettingsViewModel : BaseViewModel
    {
        #region Properties

        #endregion

        public TfaSettingsViewModel()
        {
            Title = "Two-Factor Authentication";
        }

        #region Commands

        [RelayCommand]
        async Task GoToWeb()
        {
            IsBusy = true;

            try
            {
                await Browser.Default.OpenAsync(new Uri("https://passwordmanagerwebapi.azurewebsites.net/"), BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                await AlertService.ShowToast("Error with opening URL");
            }

            IsBusy = false;
        }

        #endregion

        #region Methods

        #endregion
    }
}
