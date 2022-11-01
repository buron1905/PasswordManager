using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data.Persistance
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPasswordRepository PasswordRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IRepositoryBase<T>? GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
