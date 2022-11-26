using Models;
using Models.DTOs;

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
