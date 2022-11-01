using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data
{
    public interface IRefreshTokenService : IDataServiceBase<RefreshToken>
    {
        Task<IEnumerable<RefreshTokenDTO>> GetAllByUserIdAsync(Guid userId);
        Task<RefreshTokenDTO> GetByIdAsync(Guid userId, Guid refreshTokenId);
        Task<RefreshTokenDTO> CreateAsync(Guid userId, RefreshTokenDTO refreshTokenDTO);
        Task UpdateAsync(Guid userId, RefreshTokenDTO refreshTokenDTO);
        Task DeleteAsync(Guid userId, Guid refreshTokenId);
    }
}
