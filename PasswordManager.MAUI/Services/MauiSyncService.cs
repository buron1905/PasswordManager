using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Data;
using System.Text;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public class MauiSyncService : MauiBaseDataService, IMauiSyncService
    {

        private readonly IDataServiceWrapper _dataServiceWrapper;

        public MauiSyncService(HttpClient httpClient, IConnectivity connectivity, IDataServiceWrapper dataServiceWrapper) : base(httpClient, connectivity)
        {
            _dataServiceWrapper = dataServiceWrapper;
        }

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTime(Guid userId)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(string.Format(AppConstants.ApiUrl + "Sync", string.Empty));

                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(uri);
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

        public async Task<LastChangeResponseDTO?> GetLastChangeDateTimeLocal(Guid userId)
        {
            var lastChangeUser = (await _dataServiceWrapper.UserService.GetByIdAsync(userId)).UDT;
            var passwords = await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(userId);
            var lastChangePassword = GetLastChangeDateTime(passwords);

            var response = new LastChangeResponseDTO() { LastChangeUser = lastChangeUser, LastChangePassword = lastChangePassword };
            return response;
        }

        public async Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(AppConstants.ApiUrl + AppConstants.SyncSuffix);
                data.UserDTO.Password = "password";
                string json = JsonSerializer.Serialize<SyncRequestDTO>(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = content;
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ActiveUserService.Instance.Token);

                try
                {
                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        var syncResponseDTO = JsonSerializer.Deserialize<SyncResponseDTO>(contentResponse, _serializerOptions);
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

        private async Task SyncAccountBasedOnReceivedData(SyncResponseDTO data)
        {
            if (!data.SyncSuccessful)
                return;

            if (!data.SendingNewData)
                return;

            var userId = ActiveUserService.Instance.ActiveUser.Id;

            if (data.UserDTO.Id != userId)
                return;

            var offlinePasswords = await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(ActiveUserService.Instance.ActiveUser.Id);
            var newPasswords = data.Passwords.Where(x => !offlinePasswords.Any(o => o.Id == x.Id)).ToList();
            var updatedPasswords = data.Passwords.Where(x => offlinePasswords.Any(o => o.Id == x.Id)).ToList();

            // new passwords
            foreach (var password in newPasswords)
            {
                await _dataServiceWrapper.PasswordService.CreateAsync(userId, password);
            }

            // updated passwords
            foreach (var password in updatedPasswords)
            {
                await _dataServiceWrapper.PasswordService.UpdateAsync(userId, password);
            }
        }

        public static DateTime GetLastChangeDateTime(IEnumerable<PasswordDTO> passwords)
        {
            if (passwords.Count() == 0)
                return DateTime.MinValue;

            return passwords.Max(x => x.UDT);
        }


        // TODO - this has to work with authorization and even with 2FA, when there is internet connection
        public async Task<SyncResponseDTO?> DoSync()
        {
            if (IsNetworkAccess())
            {
                var lastChangeOnline = await GetLastChangeDateTime(ActiveUserService.Instance.ActiveUser.Id);
                var lastChangeLocal = await GetLastChangeDateTimeLocal(ActiveUserService.Instance.ActiveUser.Id);
                if (lastChangeLocal is not null && lastChangeOnline is not null)
                {
                    //if (lastChangeLocal.LastChangeUser != lastChangeOnline.LastChangeUser || lastChangeLocal.LastChangePassword != lastChangeOnline.LastChangePassword)
                    {
                        var localData = new SyncRequestDTO();
                        localData.UserDTO = await _dataServiceWrapper.UserService.GetByIdAsync(ActiveUserService.Instance.ActiveUser.Id);
                        localData.Passwords = await _dataServiceWrapper.PasswordService.GetAllByUserIdAsync(ActiveUserService.Instance.ActiveUser.Id);

                        var response = await SyncAccount(localData);

                        if (response is not null)
                        {
                            await SyncAccountBasedOnReceivedData(response);
                            return response;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<SyncResponseDTO> SyncExistingAndNewUser(UserDTO newUserDTO)
        {
            var response = new SyncResponseDTO() { SyncSuccessful = true };

            if (!await _dataServiceWrapper.UserService.AnyAsync(x => x.Id == newUserDTO.Id))
            {
                response.UserDTO = await _dataServiceWrapper.UserService.CreateAsync(newUserDTO);
                response.SendingNewData = true;
            }
            else
            {
                response.UserDTO = await _dataServiceWrapper.UserService.UpdateAsync(newUserDTO);
            }

            return response;
        }
    }
}
