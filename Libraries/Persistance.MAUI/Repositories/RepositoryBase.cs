using Models;
using Services.Abstraction.Data.Persistance;
using SQLite;
using System.Linq.Expressions;

namespace Persistance.MAUI.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Entity, new()
    {
        SQLiteAsyncConnection? _connection;

        public RepositoryBase()
        {
        }

        async Task Init()
        {
            if (_connection is not null)
            {
                var userTable = await _connection.Table<User>().ToListAsync();
                var passwordTable = await _connection.Table<Password>().ToListAsync();
                var settingsTable = await _connection.Table<Settings>().ToListAsync();
                return;
            }

            //File.Delete(Constants.DatabasePath);

            _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await _connection.CreateTableAsync<User>();
            await _connection.CreateTableAsync<Password>();
            await _connection.CreateTableAsync<Settings>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            await Init();
            return await _connection.Table<T>().CountAsync(expression) > 0;
        }

        public async void Create(T entity)
        {
            await Init();
            entity.Id = Guid.NewGuid();
            entity.IDT = DateTime.UtcNow;
            await _connection.InsertAsync(entity);
        }

        public async void Update(T entity)
        {
            await Init();
            entity.UDT = DateTime.UtcNow;
            await _connection.UpdateAsync(entity);
            var test2 = await _connection.Table<T>().ToListAsync();
        }

        public async void Delete(T entity)
        {
            await Init();
            await _connection.Table<T>().DeleteAsync(x => x.Id == entity.Id);
        }

        public async void DeleteAll(Expression<Func<T, bool>> expression)
        {
            await Init();
            await _connection.Table<T>().DeleteAsync(expression);
        }

        public async Task<IQueryable<T>> FindAll()
        {
            await Init();
            return (await _connection.Table<T>().ToListAsync()).AsQueryable();
        }

        public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            await Init();
            return (await _connection.Table<T>().Where(expression).ToListAsync()).AsQueryable();
        }

        public async Task<T> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
        {
            await Init();
            return await _connection.Table<T>().Where(expression).FirstOrDefaultAsync();
        }

    }
}
