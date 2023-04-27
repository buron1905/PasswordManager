using Models;
using Services.Abstraction.Data.Persistance;
using SQLite;
using System.Linq.Expressions;

namespace Persistance.MAUI.Repositories
{
    public abstract class MauiRepositoryBase<T> : IRepositoryBase<T> where T : Entity, new()
    {
        SQLiteAsyncConnection? _connection;

        public MauiRepositoryBase()
        {
        }

        async Task Init()
        {
            if (_connection is not null)
                return;

            //File.Delete(Constants.DatabasePath);

            _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await _connection.CreateTableAsync<User>();
            await _connection.CreateTableAsync<Password>();
            await _connection.CreateTableAsync<Settings>();
        }

        bool IsFromServerOrUpToDate(T entity)
        {
            return entity.Id != Guid.Empty
                && entity.UDT == entity.UDTLocal;
        }

        bool IsOnlyLocal(T entity)
        {
            return entity.UDT == DateTime.MinValue && entity.UDTLocal != DateTime.MinValue;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            await Init();
            return await _connection.Table<T>().CountAsync(expression) > 0;
        }

        public async Task Create(T entity)
        {
            await Init();

            if (!IsFromServerOrUpToDate(entity))
            {
                entity.Id = Guid.NewGuid();
                entity.IDT = DateTime.UtcNow;
                entity.UDTLocal = entity.IDT;
                entity.UDT = DateTime.MinValue; // This will be updated to server time when syncing
            }

            var tableBefore = await _connection.Table<T>().ToListAsync();
            await _connection.InsertOrReplaceAsync(entity);
            var tableAfter = await _connection.Table<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            await Init();
            if (!IsFromServerOrUpToDate(entity))
                entity.UDTLocal = DateTime.UtcNow;

            var tableBefore = await _connection.Table<T>().ToListAsync();
            await _connection.UpdateAsync(entity);
            var tableAfter = await _connection.Table<T>().ToListAsync();
        }

        public async Task Delete(T entity)
        {
            await Init();

            if ((IsFromServerOrUpToDate(entity) && entity.Deleted)
                || IsOnlyLocal(entity))
            {
                var tableBefore = await _connection.Table<T>().ToListAsync();
                await _connection.Table<T>().DeleteAsync(x => x.Id == entity.Id);
                var tableAfter = await _connection.Table<T>().ToListAsync();
            }
            else
            {
                entity.UDTLocal = DateTime.UtcNow;
                entity.DDT = entity.UDTLocal;
                entity.Deleted = true;
                await _connection.UpdateAsync(entity);
            }
        }

        public async Task DeleteAll(Expression<Func<T, bool>> expression)
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
