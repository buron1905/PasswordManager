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

        public Task<bool> ConfirmEmailAsync(string email, string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> DecryptString(string password, string textEncrypted)
        {
            throw new NotImplementedException();
        }

        public Task<TfaSetupDTO> DisableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
        {
            throw new NotImplementedException();
        }

        public Task<TfaSetupDTO> EnableTfa(Guid userId, string password, TfaSetupDTO tfaSetupDTO)
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

        public Task<TfaSetupDTO> GetTfaSetup(Guid userId, string password)
        {
            throw new NotImplementedException();
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

        public Task<AuthResponseDTO> LoginTfaAsync(LoginTfaRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> LoginTfaAsync(string code, string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }

        public void ResendConfirmEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorDisabledAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TfaSetupDTO?> SetTwoFactorEnabledAsync(Guid userId, string password)
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

        Task IAuthService.ResendConfirmEmail(string email)
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
    }
}
