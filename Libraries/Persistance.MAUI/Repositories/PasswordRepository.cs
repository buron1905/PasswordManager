using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class PasswordRepository : RepositoryBase<Password>, IPasswordRepository
    {
        public PasswordRepository()
        {
        }

        public async Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid userId)
        {
            return FindByCondition(a => a.UserId.Equals(userId)) as IEnumerable<Password>;
        }

        public Task<Password?> GetByIdAsync(Guid passwordId)
        {
            return FindSingleOrDefaultByCondition(password => password.Id.Equals(passwordId));
        }

        public Task<Password?> GetPasswordWithUserAsync(Guid passwordId)
        {
            throw new NotImplementedException();
            //return FindByCondition(password => password.Id.Equals(passwordId))
            //    .Include(password => password.User)
            //    .FirstOrDefaultAsync();
        }
    }
}
