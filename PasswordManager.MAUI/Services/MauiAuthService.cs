﻿using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Auth;
using Services.Cryptography;
using Services.TMP;
using System.Text;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public class MauiAuthService : MauiBaseDataService, IMauiAuthService
    {
        readonly IAuthService _offlineAuthService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public MauiAuthService(HttpClient httpClient, IConnectivity connectivity, ITwoFactorAuthService twoFactorAuthService, IAuthService offlineAuthService) : base(httpClient, connectivity)
        {
            _offlineAuthService = offlineAuthService;
            _twoFactorAuthService = twoFactorAuthService;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            throw new NotImplementedException("Email confirmation is only possible via web application.");
        }

        public async Task<string> GetTfaCode()
        {
            return _twoFactorAuthService.GetCurrentPIN(ActiveUserService.Instance.ActiveUser.TwoFactorSecret);
        }

        public async Task<TfaSetupDTO> GetTfaSetup(Guid userId)
        {
            return await _offlineAuthService.GetTfaSetup(userId);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO requestDTO)
        {
            if (!IsNetworkAccess())
            {
                // In offline login token removed
                var authResponse = await _offlineAuthService.LoginAsync(requestDTO);
                if (authResponse is not null)
                    authResponse.JweToken = null;

                return authResponse;
            }


            requestDTO.Password = EncryptionService.EncryptUsingRSA(requestDTO.Password, EncryptionKeys.publicRsaKey);

            Uri uri = new Uri(AppConstants.ApiUrl + AppConstants.LoginSuffix);
            string json = JsonSerializer.Serialize(requestDTO);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = await response.Content.ReadAsStringAsync();
                    var authResponseDTO = JsonSerializer.Deserialize<AuthResponseDTO>(contentResponse, _serializerOptions);

                    return authResponseDTO;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AuthResponseDTO> LoginWithTfaAsync(LoginWithTfaRequestDTO requestDTO)
        {
            if (!IsNetworkAccess())
            {
                // In offline login token removed
                var authResponse = await _offlineAuthService.LoginWithTfaAsync(requestDTO);
                if (authResponse is not null)
                    authResponse.JweToken = null;
                return authResponse;
            }

            requestDTO.Password = EncryptionService.EncryptUsingRSA(requestDTO.Password, EncryptionKeys.publicRsaKey);

            Uri uri = new Uri(AppConstants.ApiUrl + AppConstants.LoginWithTfaSuffix);
            string json = JsonSerializer.Serialize(requestDTO);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = await response.Content.ReadAsStringAsync();
                    var authResponseDTO = JsonSerializer.Deserialize<AuthResponseDTO>(contentResponse, _serializerOptions);

                    return authResponseDTO;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<RegisterResponseDTO?> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            if (!IsNetworkAccess())
                return null;

            requestDTO.Password = EncryptionService.EncryptUsingRSA(requestDTO.Password, EncryptionKeys.publicRsaKey);
            requestDTO.ConfirmPassword = EncryptionService.EncryptUsingRSA(requestDTO.ConfirmPassword, EncryptionKeys.publicRsaKey);

            Uri uri = new Uri(string.Format(AppConstants.ApiUrl + AppConstants.RegisterSuffix));
            string json = JsonSerializer.Serialize(requestDTO);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = await response.Content.ReadAsStringAsync();
                    var registerResponseDTO = JsonSerializer.Deserialize<RegisterResponseDTO>(contentResponse, _serializerOptions);

                    return registerResponseDTO;
                }
                else
                    return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public bool TokenIsValid(string token)
        {
            return _offlineAuthService.TokenIsValid(token);
        }

        public bool ValidateTfaCode(string secret, string code)
        {
            return _offlineAuthService.ValidateTfaCode(secret, code);
        }

        public async Task<HttpRequestMessage> AddAuthorizationHeaderToRequest(HttpRequestMessage request)
        {
            if (!string.IsNullOrWhiteSpace(ActiveUserService.Instance.Token) && ActiveUserService.Instance.TokenExpirationDateTime > DateTime.UtcNow)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ActiveUserService.Instance.Token);
                return request;
            }

            if (IsNetworkAccess())
            {
                var loginRequest = new LoginWithTfaRequestDTO
                {
                    EmailAddress = ActiveUserService.Instance.ActiveUser.EmailAddress,
                    Password = ActiveUserService.Instance.ActiveUser.Password,
                    Code = await GetTfaCode()
                };
                var authResponse = await LoginWithTfaAsync(loginRequest);
                if (authResponse is not null)
                {

                    authResponse.User.Password = ActiveUserService.Instance.ActiveUser.Password;

                    ActiveUserService.Instance.Login(authResponse.User, ActiveUserService.Instance.CipherKey);
                    ActiveUserService.Instance.Token = authResponse.JweToken;
                    ActiveUserService.Instance.TokenExpirationDateTime = authResponse.ExpirationDateTime;

                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ActiveUserService.Instance.Token);
                }
            }

            return request;
        }

        public bool ValidateMasterPassword(string password)
        {
            return _offlineAuthService.ValidateMasterPassword(password);
        }

        public AuthResponseDTO GetAuthResponse(UserDTO user, string emailAddress, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            return _offlineAuthService.GetAuthResponse(user, emailAddress, isAuthSuccessful, tfaEnabled, tfaChecked, emailVerified);
        }

        public Task<TfaSetupDTO> EnableTfa(Guid userId, TfaSetupDTO tfaSetupDTO)
        {
            throw new NotImplementedException("Only through web app");
        }

        public Task<TfaSetupDTO> DisableTfa(Guid userId, TfaSetupDTO tfaSetupDTO)
        {
            throw new NotImplementedException("Only through web app");
        }

        public async Task<bool> ResendConfirmEmail(string email)
        {
            return await _offlineAuthService.ResendConfirmEmail(email);
        }

        public async Task<UserDTO> SetTwoFactorEnabledAsync(Guid userId)
        {
            return await _offlineAuthService.SetTwoFactorEnabledAsync(userId);
        }

        public async Task<UserDTO> SetTwoFactorDisabledAsync(Guid userId)
        {
            return await _offlineAuthService.SetTwoFactorDisabledAsync(userId);
        }

        public TfaSetupDTO GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            return _offlineAuthService.GenerateTfaSetupDTO(issuer, accountTitle, accountSecretKey);
        }
    }
}
