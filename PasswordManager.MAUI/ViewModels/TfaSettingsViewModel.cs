﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        [NotifyPropertyChangedFor(nameof(QrCodeSetupImageBytes))]
        string _qrCodeSetupImageUrl;

        [ObservableProperty]
        byte[]? _qrCodeSetupImageBytes;

        IAuthService _authService;

        #endregion

        public TfaSettingsViewModel(IAuthService authService)
        {
            Title = "Two-Factor Authentication";
            _authService = authService;

            TfaEnabled = false;
            Task.WaitAll(GetTfaSetup());
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
                SetQrCodeBytes();
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
            SetQrCodeBytes();
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
            SetQrCodeBytes();
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

        #region Methods

        void SetQrCodeBytes()
        {
            int prefixLength = QrCodeSetupImageUrl.IndexOf(',') + 1;
            var QrCodeSetupImageUrlSubstring = QrCodeSetupImageUrl.Substring(prefixLength);
            QrCodeSetupImageBytes = Convert.FromBase64String(QrCodeSetupImageUrlSubstring);
        }

        #endregion
    }
}