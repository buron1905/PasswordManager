using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class SettingsRepository : RepositoryBase<Settings>, ISettingsRepository
    {
        public SettingsRepository()
        {
        }

        public Task<Settings?> GetSettingsByUser(Guid ownerId)
        {
            return FindSingleOrDefaultByCondition(settings => settings.UserId.Equals(ownerId));
        }

        public Task<Settings?> GetByIdAsync(Guid settingsId)
        {
            return FindSingleOrDefaultByCondition(settings => settings.Id.Equals(settingsId));
        }

        public Task<Settings?> GetSettingsWithUserAsync(Guid settingsId)
        {
            throw new NotImplementedException();
            //return FindByCondition(settings => settings.Id.Equals(settingsId))
            //    .Include(settings => settings.User)
            //    .FirstOrDefaultAsync();
        }
    }
}
