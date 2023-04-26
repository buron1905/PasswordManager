using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class MauiSettingsRepository : MauiRepositoryBase<Settings>, ISettingsRepository
    {
        public MauiSettingsRepository() { }

        public async Task<Settings?> GetSettingsByUser(Guid ownerId)
        {
            return await FindSingleOrDefaultByCondition(settings => settings.UserId.Equals(ownerId));
        }

        public async Task<Settings?> GetByIdAsync(Guid settingsId)
        {
            return await FindSingleOrDefaultByCondition(settings => settings.Id.Equals(settingsId));
        }

    }
}
