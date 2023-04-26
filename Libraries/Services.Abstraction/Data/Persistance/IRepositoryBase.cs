using System.Linq.Expressions;

namespace Services.Abstraction.Data.Persistance
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAll();
        Task<IQueryable<T?>> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeleteAll(Expression<Func<T, bool>> expression);
    }
}
