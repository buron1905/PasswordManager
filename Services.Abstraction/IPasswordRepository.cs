using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IPasswordRepository : IRepositoryBase<Password>
    {
        Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid userId);
        Task<Password?> GetByIdAsync(Guid passwordId);
        Task<Password?> GetPasswordWithUserAsync(Guid passwordId);
    }
}
