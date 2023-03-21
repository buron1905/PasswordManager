using Microsoft.EntityFrameworkCore;
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
            return await (await FindAll())
               .OrderBy(u => u.EmailAddress)
               .ToListAsync();
        }

        public Task<User?> GetByIdAsync(Guid userId)
        {
            return FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return FindSingleOrDefaultByCondition(user => user.EmailAddress.Equals(email));
        }

        public async Task<User?> GetUserWithPasswordsAndSettingsAsync(Guid userId)
        {
            throw new NotImplementedException();
            //return await (await FindByCondition(user => user.Id.Equals(userId)))
            //    .Include(user => user.Passwords)
            //    .Include(user => user.Settings)
            //    .FirstOrDefaultAsync();
        }

    }
}
