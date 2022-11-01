using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<Settings?> GetSettingsWithUserAsync(Guid settingsId)
        {
            return FindByCondition(settings => settings.Id.Equals(settingsId))
                .Include(settings => settings.User)
                .FirstOrDefaultAsync();
        }
    }
}
