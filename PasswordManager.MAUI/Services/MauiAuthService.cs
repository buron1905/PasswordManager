using Models.DTOs;
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

        public async Task<TfaSetupDTO> DisableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<TfaSetupDTO> EnableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
        {
            throw new NotImplementedException();
        }

        public string GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            throw new NotImplementedException();
        }

        public AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true)
        {
            throw new NotImplementedException();
        }

        public AuthResponseDTO GetAuthResponse(Guid userId, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            throw new NotImplementedException();
        }

        public AuthResponseDTO GetAuthResponse(UserDTO user, string emailAddress, string password, bool isAuthSuccessful = true, bool tfaEnabled = false, bool tfaChecked = true, bool emailVerified = true)
        {
            throw new NotImplementedException();
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

        public async Task<AuthResponseDTO> LoginTfaAsync(LoginTfaRequestDTO requestDTO)
        {
            if (!IsNetworkAccess())
            {
                // In offline login token removed
                var authResponse = await _offlineAuthService.LoginTfaAsync(requestDTO);
                if (authResponse is not null)
                    authResponse.JweToken = null;
                return authResponse;
            }

            return null;
        }

        public async Task<AuthResponseDTO> LoginTfaAsync(string code, string email, string password)
        {
            throw new NotImplementedException();
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

        public async Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
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

        public void ResendConfirmEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task SetTwoFactorDisabledAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TfaSetupDTO?> SetTwoFactorEnabledAsync(Guid userId, string password)
        {
            throw new NotImplementedException();
        }

        public bool TokenIsValid(string token)
        {
            throw new NotImplementedException();
        }

        public bool ValidateTfaCode(string secret, string code)
        {
            throw new NotImplementedException();
        }

        TfaSetupDTO IAuthService.GenerateTfaSetupDTO(string issuer, string accountTitle, string accountSecretKey)
        {
            throw new NotImplementedException();
        }

        Task<bool> IAuthService.ResendConfirmEmail(string email)
        {
            throw new NotImplementedException();
        }

        Task<UserDTO> IAuthService.SetTwoFactorDisabledAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<UserDTO> IAuthService.SetTwoFactorEnabledAsync(Guid userId, string password)
        {
            throw new NotImplementedException();
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

        public string GetCipherKeySHA256Value(string plainPassword)
        {
            return _offlineAuthService.GetCipherKeySHA256Value(plainPassword);
        }
    }
}
