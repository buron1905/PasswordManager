using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return (await FindAll()).OrderBy(u => u.EmailAddress);
        }

        public async Task<User?> FindByIdAsync(Guid userId)
        {
            return await FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await FindSingleOrDefaultByCondition(user => user.EmailAddress!.Equals(email));
        }

    }
}
