using Models;

namespace Services.Abstraction.Data.Persistance
{
    public interface ISettingsRepository : IRepositoryBase<Settings>
    {
        Task<Settings> GetSettingsByUser(Guid userId);
        Task<Settings?> GetByIdAsync(Guid settingsId);
        Task<Settings?> GetSettingsWithUserAsync(Guid settingsId);
    }
}
