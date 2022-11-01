using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data.Persistance
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T?> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(Expression<Func<T, bool>> expression);
    }
}
