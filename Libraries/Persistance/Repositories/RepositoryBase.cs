using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction.Data.Persistance;
using System.Linq.Expressions;

namespace Persistance.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Entity
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

        bool IsFromLocalDB(T entity)
        {
            return entity.UDT != entity.UDTLocal;
        }

        public void Create(T entity)
        {
            if (!IsFromLocalDB(entity))
            {
                entity.IDT = DateTime.UtcNow;
                entity.UDT = entity.IDT;
            }
            else
            {
                entity.UDT = DateTime.UtcNow;
            }

            entity.UDTLocal = entity.UDT;

            _dataContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            entity.UDT = DateTime.UtcNow;
            entity.UDTLocal = entity.UDT;
            _dataContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.DDT = DateTime.UtcNow;
            entity.Deleted = true;
            _dataContext.Set<T>().Remove(entity);
            // TODO- _dataContext.SaveChangesAsync
        }

        public void DeleteAll(Expression<Func<T, bool>> expression)
        {
            var entities = _dataContext.Set<T>().Where(expression);
            foreach (var entity in entities)
            {
                entity.DDT = DateTime.UtcNow;
                entity.Deleted = true;
            }

            _dataContext.Set<T>().RemoveRange(entities);
        }
    }
}
