using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class PasswordRepository : RepositoryBase<Password>, IPasswordRepository
    {
        public PasswordRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid ownerId)
        {
            return await FindByCondition(a => a.UserId.Equals(ownerId)).ToListAsync();
        }

        public Task<Password?> GetByIdAsync(Guid passwordId)
        {
            return FindSingleOrDefaultByCondition(password => password.Id.Equals(passwordId));
        }

        public Task<Password?> GetPasswordWithUserAsync(Guid passwordId)
        {
            return FindByCondition(password => password.Id.Equals(passwordId))
                .Include(password => password.User)
                .FirstOrDefaultAsync();
        }
    }
}
