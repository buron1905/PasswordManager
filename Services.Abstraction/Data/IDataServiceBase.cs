using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Data
{
    public interface IDataServiceBase<T>
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression);
        Task RemoveAll(Expression<Func<T, bool>> expression);
    }
}
