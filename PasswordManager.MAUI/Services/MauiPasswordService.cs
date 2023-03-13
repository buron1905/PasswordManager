using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using System.Linq.Expressions;

namespace PasswordManager.MAUI.Services
{
    public class MauiPasswordService : MauiBaseDataService, IPasswordService
    {
        readonly IPasswordService _offlinePasswordService;

        public MauiPasswordService(HttpClient httpClient, IConnectivity connectivity, IPasswordService offlinePasswordService) : base(httpClient, connectivity)
        {
            _offlinePasswordService = offlinePasswordService;
        }

        public Task<bool> AnyAsync(Expression<Func<Password, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid userId, Guid passwordId)
        {
            throw new NotImplementedException();
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
