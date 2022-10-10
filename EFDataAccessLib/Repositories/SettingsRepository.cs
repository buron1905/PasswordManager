using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLib.Repositories
{
    public class SettingsRepository : RepositoryBase<Settings>, ISettingsRepository
    {
        public SettingsRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<Settings?> GetSettingsByUser(Guid ownerId)
        {
            return await FindByCondition(a => a.UserId.Equals(ownerId)).FirstOrDefaultAsync();
        }

        public async Task<Settings?> GetByIdAsync(Guid settingsId)
        {
            return await FindByCondition(settings => settings.Id.Equals(settingsId))
                .FirstOrDefaultAsync();
        }

        public async Task<Settings?> GetSettingsWithUserAsync(Guid settingsId)
        {
            return await FindByCondition(settings => settings.Id.Equals(settingsId))
                .Include(settings => settings.User)
                .FirstOrDefaultAsync();
        }
    }
}
