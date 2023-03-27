using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTOs;
using PasswordManager.MAUI.Services;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.ViewModels
{
    public partial class TfaSettingsViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TfaNotEnabled))]
        bool _tfaEnabled;
        public bool TfaNotEnabled => !TfaEnabled;

        [ObservableProperty]
        string _code;

        [ObservableProperty]
        string _authKey;

        [ObservableProperty]
        string _qrCodeSetupImageUrl;

        IAuthService _authService;

        #endregion

        public TfaSettingsViewModel(IAuthService authService)
        {
            Title = "Two-Factor Authentication";
            _authService = authService;

            TfaEnabled = false;
            AuthKey = "MZSDSMZQGFRWIMRS";
            QrCodeSetupImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKsAAACrAQAAAAAxk1G0AAAC1ElEQVR4nOWXMa6sOBBFCxE442/AEttw5i3BBqB7A82WnHkbSN4AZAQWnuMHrZf8pEhmpEEIdR9aBlfVvVUt5W/HKv9vvIlMJpWQPt6Osd2DNHq8lzy43Bz9x6Q9ShMhehzsJO07tqWIuPSOMj7CIy91SBNkkHZ5jEu/OSuSx5LKM1zyJOVl1jmkUvL8u3kFJt5jJNLf8zcNCsyxmfSSPIfy8jL/FoQCn04GZ5vALtfxyJNpFz3eTPtxVNB6nYNZ/+hxOeRPyJMj7dJ5QrVeu1ThvaTlKO/D1uRHAv8tZA3ejEwUcpAZKZR2ObjqcQ05ecuEitfsZJ31mBQ1MZWDvPV7seLtnXkN3kP5eDbX7kemfJqw3vWtwbUGwzr43NVVexziAS6xXcBiById+3dA5Wq8GTu5dnPstUph8LekVPj0iTJ8cce3H5+WkK76VuHNIIKy1zOd0p7m+0gNLoECrA7RubUTK8ZemVdhAky2SZfUaKVNsC41LrHf8d0DMaFy3u6bNB22g+s/LBkT0drJmx7v+K6vZjNHssfCX+vVYapmnbyISafL1NGteQ1Gl68aoYzbYTPcv+tbhflYu6ut1ivSmcurdJh4b65ll2IIOe941aAOn3SzqmxKGIfgmn/W1uESy8eRqDwfaXP9wh09rv5k+Ia+8e8e672TpsGbpy33S6S7Csck9SdqLLbDwo3tat6w3m+L1uCCOR2Jj03IOB9xutSgwhvVJzXzE/b5s+PrkTpM3nAFhCUUoB18f8+DGnx6uhBPYHQCMNDdBaHDV1tD2VK9Ey9/gLdavy0uji5fpkcQD/DP7EO0Vuav2goK448a09OwOuRIG+Fmc8+DOsw8OLKqZ22Lyt+lvQ1PhQOuyaCBuO3MtXb7J3gM/UlPo5nQJBlRH+EJ6zVlwSRIfh2Z9Zj/DZ5II820h/6beR2u8T4qPnGs2pGqvLT4X/if9h/H/wCbVRzJWIktnQAAAABJRU5ErkJggg==";
            //Task.WaitAll(GetTfaSetup());
        }

        #region Commands

        [RelayCommand]
        async Task GetTfaSetup()
        {
            IsBusy = true;
            try
            {
                var result = await _authService.GetTfaSetup(ActiveUserService.Instance.UserDTO.Id, ActiveUserService.Instance.Password);


                if (result is null)
                {
                    await PopupService.ShowToast("Error.");
                    IsBusy = false;
                    return;
                }

                TfaEnabled = result.IsTfaEnabled;
                AuthKey = result.AuthenticatorKey;
                QrCodeSetupImageUrl = result.QrCodeSetupImageUrl;
            }
            catch (Exception ex)
            {
                await PopupService.ShowToast(ex.Message);
            }

            IsBusy = false;
        }

        [RelayCommand]
        async Task Enable()
        {
            IsBusy = true;

            var result = await _authService.EnableTfa(ActiveUserService.Instance.UserDTO.Id, ActiveUserService.Instance.Password, new TfaSetupDTO() { Code = Code });
            if (result is null)
            {
                await PopupService.ShowToast("Wrong Code.");
                IsBusy = false;
                return;
            }

            TfaEnabled = result.IsTfaEnabled;
            AuthKey = result.AuthenticatorKey;
            QrCodeSetupImageUrl = result.QrCodeSetupImageUrl;
            Code = "";
            await PopupService.ShowToast("Enabled");

            TfaEnabled = true;
            IsBusy = false;
        }

        [RelayCommand]
        async Task Disable()
        {
            IsBusy = true;

            var result = await _authService.DisableTfa(ActiveUserService.Instance.UserDTO.Id, ActiveUserService.Instance.Password, new TfaSetupDTO() { Code = Code });

            if (result is null)
            {
                await PopupService.ShowToast("Wrong Code.");
                IsBusy = false;
                return;
            }

            TfaEnabled = result.IsTfaEnabled;
            AuthKey = result.AuthenticatorKey;
            QrCodeSetupImageUrl = result.QrCodeSetupImageUrl;
            Code = "";
            await PopupService.ShowToast("Disabled");

            TfaEnabled = false;
            IsBusy = false;
        }

        [RelayCommand]
        async Task Copy()
        {
            await Clipboard.SetTextAsync(AuthKey);
            await PopupService.ShowToast("Copied to clipboard");
        }

        #endregion
    }
}
