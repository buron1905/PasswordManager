using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class MauiPasswordRepository : MauiRepositoryBase<Password>, IPasswordRepository
    {
        public MauiPasswordRepository() { }

        public async Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid userId)
        {
            return await FindByCondition(a => a.UserId.Equals(userId));
        }

        public async Task<Password?> FindByIdAsync(Guid passwordId)
        {
            return await FindSingleOrDefaultByCondition(password => password.Id.Equals(passwordId));
        }

    }
}
