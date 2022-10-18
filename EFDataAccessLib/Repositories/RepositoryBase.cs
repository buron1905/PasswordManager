using Microsoft.EntityFrameworkCore;
using Models;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext _dataContext;
        
        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public IQueryable<T> FindAll()
        {
            return _dataContext.Set<T>().AsNoTracking();
        }
        
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().Where(expression).AsNoTracking();
        }

        public Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().SingleOrDefaultAsync(expression); //.AsNoTracking();
        }
        
        public async Task<bool> CheckIfExistsAnyByCondition(Expression<Func<T, bool>> expression)
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
    }
}
