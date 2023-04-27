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

    }
}
