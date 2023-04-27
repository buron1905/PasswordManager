using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.Repositories
{
    public class SettingsRepository : RepositoryBase<Settings>, ISettingsRepository
    {
        public SettingsRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Settings?> GetSettingsByUser(Guid ownerId)
        {
            return await FindSingleOrDefaultByCondition(settings => settings.UserId.Equals(ownerId));
        }

        public async Task<Settings?> FindByIdAsync(Guid settingsId)
        {
            return await FindSingleOrDefaultByCondition(settings => settings.Id.Equals(settingsId));
        }

    }
}
