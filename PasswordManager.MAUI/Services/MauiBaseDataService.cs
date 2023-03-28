using PasswordManager.MAUI.Handlers;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public abstract class MauiBaseDataService
    {
        protected HttpClient _httpClient;
        JsonSerializerOptions _serializerOptions;
        protected readonly IConnectivity _connectivity;
        IHttpsClientHandlerService _httpsClientHandlerService;

        public MauiBaseDataService(HttpClient httpClient, IConnectivity connectivity, IHttpsClientHandlerService service = null)
        {
            //_httpClient = httpClient;
            _connectivity = connectivity;


#if DEBUG
            _httpsClientHandlerService = service;
            HttpMessageHandler handler = _httpsClientHandlerService.GetPlatformMessageHandler();
            if (handler != null)
                _httpClient = new HttpClient(handler);
            else
                _httpClient = new HttpClient();
#else
            _httpClient = new HttpClient();
#endif
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
