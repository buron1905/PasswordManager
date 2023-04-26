using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class MauiUserRepository : MauiRepositoryBase<User>, IUserRepository
    {

        public MauiUserRepository() { }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return (await FindAll()).OrderBy(u => u.EmailAddress);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await FindSingleOrDefaultByCondition(user => user.EmailAddress!.Equals(email));
        }

    }
}
