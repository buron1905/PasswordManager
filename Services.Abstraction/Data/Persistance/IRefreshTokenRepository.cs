using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data.Persistance
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId);
        Task<RefreshToken?> GetByIdAsync(Guid refreshTokenId);
        Task<RefreshToken?> GetRefreshTokenWithUserAsync(Guid refreshTokenId);
    }
}
