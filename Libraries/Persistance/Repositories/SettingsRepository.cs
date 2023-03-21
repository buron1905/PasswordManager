﻿using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.Repositories
{
    public class SettingsRepository : RepositoryBase<Settings>, ISettingsRepository
    {
        public SettingsRepository(DataContext repositoryContext)
            : base(repositoryContext)
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

        public async Task<Settings?> GetSettingsWithUserAsync(Guid settingsId)
        {
            throw new NotImplementedException();
            //return await (await FindByCondition(settings => settings.Id.Equals(settingsId)))
            //    .Include(settings => settings.User)
            //    .FirstOrDefaultAsync();
        }
    }
}
