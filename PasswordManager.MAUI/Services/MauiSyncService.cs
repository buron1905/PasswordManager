using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Data;
using System.Text;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public class MauiSyncService : MauiBaseDataService, ISyncService
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

        public async Task<SyncResponseDTO?> SyncAccount(SyncRequestDTO data)
        {
            if (IsNetworkAccess())
            {


                Uri uri = new Uri(string.Format(AppConstants.ApiUrl, "Sync"));
                string json = JsonSerializer.Serialize<SyncRequestDTO>(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        var syncResponseDTO = JsonSerializer.Deserialize<SyncResponseDTO>(contentResponse);
                        await SyncAccountBasedOnReceivedData(syncResponseDTO);
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
    }
}
