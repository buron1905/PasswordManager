using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface ISettingsRepository : IRepositoryBase<Settings>
    {
        Task<Settings> GetSettingsByUser(Guid userId);
        Task<Settings?> GetByIdAsync(Guid settingsId);
        Task<Settings?> GetSettingsWithUserAsync(Guid settingsId);
    }
}
