using Models;

namespace Services.Abstraction.Data.Persistance
{
    public interface IPasswordRepository : IRepositoryBase<Password>
    {
        Task<IEnumerable<Password>> GetAllByUserIdAsync(Guid userId);
        Task<Password?> FindByIdAsync(Guid passwordId);
    }
}
