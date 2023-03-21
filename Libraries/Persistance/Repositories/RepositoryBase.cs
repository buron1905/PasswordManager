using Microsoft.EntityFrameworkCore;
using Services.Abstraction.Data.Persistance;
using System.Linq.Expressions;

namespace Persistance.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext _dataContext;

        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<IQueryable<T>> FindAll()
        {
            return Task.FromResult(_dataContext.Set<T>().AsNoTracking());
        }

        public Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(_dataContext.Set<T>().Where(expression).AsNoTracking());
        }

        public Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().SingleOrDefaultAsync(expression); //.AsNoTracking();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dataContext.Set<T>().AnyAsync(expression);
        }

        public void Create(T entity)
        {
            _dataContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dataContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }

        public void DeleteAll(Expression<Func<T, bool>> expression)
        {
            var entities = _dataContext.Set<T>().Where(expression);
            _dataContext.Set<T>().RemoveRange(entities);
        }
    }
}
