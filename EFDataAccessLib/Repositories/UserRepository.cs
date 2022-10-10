using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLib.Repositories
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

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await FindByCondition(user => user.Id.Equals(userId))
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithPasswordsAndSettingsAsync(Guid userId)
        {
            return await FindByCondition(user => user.Id.Equals(userId))
                .Include(user => user.Passwords)
                .Include(user => user.Settings)
                .FirstOrDefaultAsync();
        }

    }
}
