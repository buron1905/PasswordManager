using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await FindAll()
               .OrderBy(u => u.Email)
               .ToListAsync();
        }

        public Task<User?> GetByIdAsync(Guid userId)
        {
            return FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return FindSingleOrDefaultByCondition(user => user.Email.Equals(email));
        }

        public Task<User?> GetUserWithPasswordsAndSettingsAsync(Guid userId)
        {
            return FindByCondition(user => user.Id.Equals(userId))
                .Include(user => user.Passwords)
                .Include(user => user.Settings)
                .FirstOrDefaultAsync();
        }

    }
}
