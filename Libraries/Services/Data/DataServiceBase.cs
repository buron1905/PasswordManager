using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using System.Linq.Expressions;

namespace Services.Data
{
    public abstract class DataServiceBase<T> : IDataServiceBase<T>
    {
        protected IRepositoryBase<T> _repository;

        public DataServiceBase(IRepositoryBase<T> repository) => _repository = repository;

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
        {
            return await _repository.FindSingleOrDefaultByCondition(expression);
        }

        public async Task RemoveAll(Expression<Func<T, bool>> expression)
        {
            await _repository.DeleteAll(expression);
        }
    }
}
