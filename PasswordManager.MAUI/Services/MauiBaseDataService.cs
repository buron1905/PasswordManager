using Models.DTOs;
using Services.Abstraction.Auth;
using Services.Abstraction.Data.Persistance;
using Services.Data;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public abstract class MauiBaseDataService
    {
        protected HttpClient _httpClient;
        protected JsonSerializerOptions _serializerOptions;
        protected readonly IConnectivity _connectivity;

        public MauiBaseDataService(HttpClient httpClient, IConnectivity connectivity)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public bool IsNetworkAccess()
        {
            return _connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }

    public abstract class MauiBaseDataService<T> : DataServiceBase<T>
    {
        protected HttpClient _httpClient;
        protected JsonSerializerOptions _serializerOptions;
        protected readonly IConnectivity _connectivity;
        protected IMauiAuthService _authService;

        public MauiBaseDataService(HttpClient httpClient, IConnectivity connectivity, IRepositoryBase<T> repositoryBase, IMauiAuthService authService) : base(repositoryBase)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;
            _authService = authService;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public bool IsNetworkAccess()
        {
            return _connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public async Task<HttpRequestMessage> AddAuthorizationHeaderToRequest(HttpRequestMessage request)
        {
            if (ActiveUserService.Instance.TokenExpirationDateTime > DateTime.UtcNow)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ActiveUserService.Instance.Token);
                return request;
            }

            if (IsNetworkAccess())
            {
                var loginRequest = new LoginWithTfaRequestDTO
                {
                    EmailAddress = ActiveUserService.Instance.ActiveUser.EmailAddress,
                    Password = ActiveUserService.Instance.CipherKey,
                    Code = await _authService.GetTfaCode()
                };
                var authResponse = await _authService.LoginWithTfaAsync(loginRequest);
                if (authResponse is not null)
                {
                    ActiveUserService.Instance.Token = authResponse.JweToken;
                    ActiveUserService.Instance.TokenExpirationDateTime = authResponse.ExpirationDateTime;
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ActiveUserService.Instance.Token);
                }
            }

            return request;
        }
    }
}
