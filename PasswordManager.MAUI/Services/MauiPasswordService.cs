using Models;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using System.Text;
using System.Text.Json;

namespace PasswordManager.MAUI.Services
{
    public class MauiPasswordService : MauiBaseDataService<Password>, IMauiPasswordService
    {
        readonly IPasswordService _offlinePasswordService;

        public MauiPasswordService(HttpClient httpClient, IConnectivity connectivity, IPasswordService passwordService, IRepositoryWrapper repositoryWrapper,
            IMauiAuthService authService) : base(httpClient, connectivity, repositoryWrapper, authService)
        {
            _offlinePasswordService = passwordService;
        }

        public async Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            PasswordDTO passwordDTOToReturn = null;

            if (IsNetworkAccess())
            {
                Uri uri = new Uri(string.Format(AppConstants.ApiUrl + AppConstants.PasswordsSuffix, string.Empty));
                string json = JsonSerializer.Serialize(passwordDTO);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = content;
                request = await AddAuthorizationHeaderToRequest(request);

                try
                {
                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = await response.Content.ReadAsStringAsync();
                        passwordDTOToReturn = JsonSerializer.Deserialize<PasswordDTO>(contentResponse, _serializerOptions);
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            passwordDTOToReturn = await _offlinePasswordService.CreateAsync(userId, passwordDTOToReturn ?? passwordDTO);

            return passwordDTOToReturn;
        }

        public async Task<PasswordDTO> UpdateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid userId, Guid passwordId)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(string.Format(AppConstants.ApiUrl + AppConstants.PasswordsSuffix, passwordId));

                try
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            //_repositoryWrapper.PasswordRepository.Delete(password);
        }



        public async Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId)
        {
            return await _offlinePasswordService.GetAllByUserIdAsync(userId);
        }

        public async Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId)
        {
            return await _offlinePasswordService.GetByIdAsync(userId, passwordId);
        }

    }
}
