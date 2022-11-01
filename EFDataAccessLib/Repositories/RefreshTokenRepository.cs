using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }


        public async Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId)
        {
            return await FindByCondition(a => a.UserId.Equals(userId)).ToListAsync();
        }

        public Task<RefreshToken?> GetByIdAsync(Guid refreshTokenId)
        {
            return FindSingleOrDefaultByCondition(refreshToken => refreshToken.Id.Equals(refreshTokenId));
        }

        public Task<RefreshToken?> GetRefreshTokenWithUserAsync(Guid refreshTokenId)
        {
            return FindByCondition(refreshToken => refreshToken.Id.Equals(refreshTokenId))
                .Include(refreshToken => refreshToken.User)
                .FirstOrDefaultAsync();
        }
    }
}
