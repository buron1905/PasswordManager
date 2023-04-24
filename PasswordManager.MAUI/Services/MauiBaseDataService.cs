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
}
