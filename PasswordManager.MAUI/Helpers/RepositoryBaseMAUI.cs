namespace PasswordManager.MAUI.Helpers
{
    //public abstract class RepositoryBaseMAUI<T> : IRepositoryBase<T> where T : class, new()
    //{
    //    SQLiteAsyncConnection _connection;

    //    public RepositoryBaseMAUI()
    //    {
    //    }

    //    async Task Init()
    //    {
    //        if (_connection is not null)
    //            return;

    //        _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
    //        await _connection.CreateTableAsync<User>();
    //        await _connection.CreateTableAsync<Password>();
    //        await _connection.CreateTableAsync<Models.Settings>();
    //    }

    //    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    //    {
    //        await Init();
    //    }

    //    public async void Create(T entity)
    //    {
    //        await Init();
    //        await _connection.InsertAsync(entity);
    //    }

    //    public async void Delete(T entity)
    //    {
    //        await Init();
    //        await _connection.DeleteAsync(entity);
    //    }

    //    public async void DeleteAll(Expression<Func<T, bool>> expression)
    //    {
    //        await Init();
    //    }

    //    public async IQueryable<T> FindAll()
    //    {
    //        await Init();
    //        return _connection.Table<T>();
    //    }

    //    public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
    //    {
    //        await Init();
    //        return await _connection.Table<T>().Where(expression).ToListAsync();
    //    }

    //    public async Task<T> FindSingleOrDefaultByCondition(Expression<Func<T, bool>> expression)
    //    {
    //        await Init();
    //        return await _connection.Table<T>().Where(expression).FirstOrDefaultAsync();
    //    }

    //    public async void Update(T entity)
    //    {
    //        await Init();
    //        await _connection.UpdateAsync(entity);
    //    }
    //}
}
