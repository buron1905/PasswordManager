using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLib.Repositories
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

        public async Task<Password?> GetByIdAsync(Guid passwordId)
        {
            return await FindByCondition(password => password.Id.Equals(passwordId))
                .FirstOrDefaultAsync();
        }

        public async Task<Password?> GetPasswordWithUserAsync(Guid passwordId)
        {
            return await FindByCondition(password => password.Id.Equals(passwordId))
                .Include(password => password.User)
                .FirstOrDefaultAsync();
        }
    }
}
