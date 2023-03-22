using Models;
using Models.DTOs;
using PasswordManager.MAUI.Helpers;
using Services.Abstraction.Data;
using System.Linq.Expressions;

namespace PasswordManager.MAUI.Services
{
    public class MauiPasswordService : MauiBaseDataService, IPasswordService
    {
        private readonly IDataServiceWrapper _dataServiceWrapper;

        public MauiPasswordService(HttpClient httpClient, IConnectivity connectivity, IDataServiceWrapper dataServiceWrapper) : base(httpClient, connectivity)
        {
            _dataServiceWrapper = dataServiceWrapper;
        }

        public Task<bool> AnyAsync(Expression<Func<Password, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid userId, Guid passwordId)
        {
            if (IsNetworkAccess())
            {
                Uri uri = new Uri(string.Format(AppConstants.ApiUrl, passwordId));

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

            await _dataServiceWrapper.PasswordService.DeleteAsync(userId, passwordId);

        }

        public Task<Password> FindSingleOrDefaultByCondition(Expression<Func<Password, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAll(Expression<Func<Password, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDTO> UpdateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            throw new NotImplementedException();
        }
    }
}
