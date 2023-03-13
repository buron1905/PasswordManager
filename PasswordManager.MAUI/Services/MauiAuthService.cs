using Models.DTOs;
using Services.Abstraction.Auth;

namespace PasswordManager.MAUI.Services
{
    public class MauiAuthService : MauiBaseDataService, IAuthService
    {
        readonly IAuthService _offlineAuthService;

        public MauiAuthService(HttpClient httpClient, IConnectivity connectivity, IAuthService offlineAuthService) : base(httpClient, connectivity)
        {
            _offlineAuthService = offlineAuthService;
        }

        public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO requestDTO)
        {
            if (!IsNetworkAccess())
                return _offlineAuthService.LoginAsync(requestDTO);

            throw new NotImplementedException();

            // Online
            //var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
            //if (response.IsSuccessStatusCode)
            //{
            //    monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            //}
            // Offline
            /*using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
            */
        }

        public Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public bool TokenIsValid(string token)
        {
            throw new NotImplementedException();
        }

    }
}
