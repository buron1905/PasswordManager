using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IDataServiceWrapper
    {
        IUserService UserService { get; }
        IPasswordService PasswordService { get; }
        ISettingsService SettingsService { get; }
    }
}
