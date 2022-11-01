using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data
{
    public interface ISettingsService : IDataServiceBase<Settings>
    {
        Task<SettingsDTO> GetSettingsByUser(Guid userId);
        Task<SettingsDTO> GetByIdAsync(Guid userId, Guid settingsId);
        Task<SettingsDTO> CreateAsync(Guid userId, SettingsDTO settingsDTO);
        Task UpdateAsync(Guid userId, SettingsDTO settingsDTO);
        Task DeleteAsync(Guid userId, Guid settingsId);
    }
}
