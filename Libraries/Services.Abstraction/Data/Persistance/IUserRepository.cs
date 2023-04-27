using Models;

namespace Services.Abstraction.Data.Persistance
{
    public interface IUserRepository : IRepositoryBase<User>
    {

        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> FindByIdAsync(Guid userId);

    }
}
