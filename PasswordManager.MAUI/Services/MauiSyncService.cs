using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using System.Text;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public class MauiSyncService : MauiBaseDataService, IMauiSyncService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IMauiAuthService _authService;

        public MauiSyncService(HttpClient httpClient, IConnectivity connectivity, IUserService userService, IPasswordService passwordService, IMauiAuthService authService) : base(httpClient, connectivity)
        {
            _userService = userService;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTime(Guid userId)
        {
            var lastChangeUser = (await _userService.GetByIdAsync(userId)).UDTLocal;
            var passwords = await _passwordService.GetAllByUserIdAsync(userId);
            var lastChangePassword = GetPasswordsLastChangeDateTimeLocal(passwords);

            var response = new LastChangeResponseDTO() { LastChangeUser = lastChangeUser, LastChangePassword = lastChangePassword };
            return response;
        }

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTimeOnline(Guid userId)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(string.Format(AppConstants.ApiUrl + AppConstants.SyncSuffix, string.Empty));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
                request = await _authService.AddAuthorizationHeaderToRequest(request);

                try
                {
                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var lastChangeResponseDTO = JsonSerializer.Deserialize<LastChangeResponseDTO>(content);
                        return lastChangeResponseDTO;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(AppConstants.ApiUrl + AppConstants.SyncSuffix);
                string json = JsonSerializer.Serialize(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request = await _authService.AddAuthorizationHeaderToRequest(request);
                request.Content = content;

                try
                {
                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        var syncResponseDTO = JsonSerializer.Deserialize<SyncResponseDTO>(contentResponse, _serializerOptions);

                        if (syncResponseDTO is not null)
                            await SyncAccountLocal(syncResponseDTO);

                        return syncResponseDTO;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        private async Task SyncAccountLocal(SyncResponseDTO data)
        {
            if (!data.SyncSuccessful)
                return;

            if (!data.SendingNewData)
                return;

            var userId = ActiveUserService.Instance.ActiveUser.Id;

            if (data.UserDTO?.Id != userId)
                return;

            var offlinePasswords = await _passwordService.GetAllByUserIdAsync(ActiveUserService.Instance.ActiveUser.Id);
            var newPasswords = data.Passwords.Where(x => !offlinePasswords.Any(o => o.Id == x.Id)).ToList();
            var updatedPasswords = data.Passwords.Where(x => !x.Deleted && offlinePasswords.Any(o => o.Id == x.Id)).ToList();
            var deletedPasswords = data.Passwords.Where(x => x.Deleted && offlinePasswords.Any(o => o.Id == x.Id)).ToList();

            foreach (var password in newPasswords)
                await _passwordService.CreateAsync(userId, password);

            foreach (var password in updatedPasswords)
                await _passwordService.UpdateAsync(userId, password);

            foreach (var password in deletedPasswords)
                await _passwordService.DeleteAsync(userId, password);
        }

        public static DateTime GetPasswordsLastChangeDateTimeLocal(IEnumerable<PasswordDTO> passwords)
        {
            if (passwords.Count() == 0)
                return DateTime.MinValue;

            return passwords.Max(x => x.UDTLocal);
        }

        public async Task<SyncResponseDTO?> DoSync()
        {
            if (IsNetworkAccess())
            {
                var lastChangeOnline = await GetLastChangeDateTimeOnline(ActiveUserService.Instance.ActiveUser.Id);
                var lastChangeLocal = await GetLastChangeDateTime(ActiveUserService.Instance.ActiveUser.Id);
                if (lastChangeLocal is not null && lastChangeOnline is not null)
                {
                    if (lastChangeLocal.LastChangeUser != lastChangeOnline.LastChangeUser || lastChangeLocal.LastChangePassword != lastChangeOnline.LastChangePassword)
                    {
                        var localData = new SyncRequestDTO();
                        localData.UserDTO = await _userService.GetByIdAsync(ActiveUserService.Instance.ActiveUser.Id);
                        localData.Passwords = await _passwordService.GetAllByUserIdAsync(ActiveUserService.Instance.ActiveUser.Id);

                        var response = await SyncAccount(localData);
                    }
                }
            }
            return null;
        }

        public async Task<SyncResponseDTO> SyncExistingAndNewUser(UserDTO newUserDTO)
        {
            var response = new SyncResponseDTO() { SyncSuccessful = true };

            if (await _userService.AnyAsync(x => x.Id == newUserDTO.Id))
                response.UserDTO = await _userService.UpdateAsync(newUserDTO);
            else
            {
                response.UserDTO = await _userService.CreateAsync(newUserDTO);
                response.SendingNewData = true;
            }

            return response;
        }
    }
}
