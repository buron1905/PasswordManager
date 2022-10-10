using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPasswordRepository PasswordRepository { get; }
        ISettingsRepository SettingsRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
