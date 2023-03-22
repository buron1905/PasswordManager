using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using System.Linq.Expressions;

namespace Services.Data
{
    public abstract class DataServiceBase<T> : IDataServiceBase<T> where T : class
    {
        protected IRepositoryWrapper _repositoryWrapper;

        public DataServiceBase(IRepositoryWrapper repositoryWrapper) => _repositoryWrapper = repositoryWrapper;

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            var repository = _repositoryWrapper.GetRepository<T>();
            if (repository is null)
                return false;

            return await repository.AnyAsync(expression);
        }

        public async Task<T?> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
        {
            var repository = _repositoryWrapper.GetRepository<T>();
            if (repository is null)
                return null;

            return await repository.FindSingleOrDefaultByCondition(expression);
        }

        public async Task RemoveAll(Expression<Func<T, bool>> expression)
        {
            var repository = _repositoryWrapper.GetRepository<T>();
            if (repository is null)
                throw new AppException("Repository is null");

            repository.DeleteAll(expression);
            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
