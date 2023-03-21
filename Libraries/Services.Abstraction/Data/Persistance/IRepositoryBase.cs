using System.Linq.Expressions;

namespace Services.Abstraction.Data.Persistance
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAll();
        Task<IQueryable<T?>> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(Expression<Func<T, bool>> expression);
    }
}
