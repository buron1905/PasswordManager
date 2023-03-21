using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository()
        {
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(FindAll() as IEnumerable<User>);
        }

        public Task<User?> GetByIdAsync(Guid userId)
        {
            return FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return FindSingleOrDefaultByCondition(user => user.EmailAddress.Equals(email));
        }

        public Task<User?> GetUserWithPasswordsAndSettingsAsync(Guid userId)
        {
            throw new NotImplementedException();
            //return FindSingleOrDefaultByCondition(user => user.Id.Equals(userId));
        }

    }
}
