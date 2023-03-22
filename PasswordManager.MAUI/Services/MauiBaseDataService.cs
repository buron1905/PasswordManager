namespace PasswordManager.MAUI.Services
{
    public abstract class MauiBaseDataService
    {
        protected HttpClient _httpClient;
        protected readonly IConnectivity _connectivity;

        public MauiBaseDataService(HttpClient httpClient, IConnectivity connectivity)
        {
            _httpClient = httpClient;
            _connectivity = connectivity;
        }

        public bool IsNetworkAccess()
        {
            return _connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
