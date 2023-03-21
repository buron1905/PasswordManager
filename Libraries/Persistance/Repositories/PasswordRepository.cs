﻿using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.Repositories
{
    public class PasswordRepository : RepositoryBase<Password>, IPasswordRepository
    {
        public PasswordRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid userId)
        {
            return await (await FindByCondition(a => a.UserId.Equals(userId))).ToListAsync();
        }

        public Task<Password?> GetByIdAsync(Guid passwordId)
        {
            return FindSingleOrDefaultByCondition(password => password.Id.Equals(passwordId));
        }

        public async Task<Password?> GetPasswordWithUserAsync(Guid passwordId)
        {
            throw new NotImplementedException();
            //return await (await FindByCondition(password => password.Id.Equals(passwordId)))
            //    .Include(password => password.User)
            //    .FirstOrDefaultAsync();
        }
    }
}
