namespace Services.Abstraction.Data.Persistance
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPasswordRepository PasswordRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IRepositoryBase<T>? GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
