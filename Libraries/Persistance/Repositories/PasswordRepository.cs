using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.Repositories
{
    public class PasswordRepository : RepositoryBase<Password>, IPasswordRepository
    {
        public PasswordRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }

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
