using Models;
using Services.Abstraction.Data.Persistance;

namespace Persistance.MAUI.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly Lazy<IUserRepository> _lazyUserRepository;
        private readonly Lazy<IPasswordRepository> _lazyPasswordRepository;
        private readonly Lazy<ISettingsRepository> _lazySettingsRepository;

        public RepositoryWrapper()
        {
            _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository());
            _lazyPasswordRepository = new Lazy<IPasswordRepository>(() => new PasswordRepository());
            _lazySettingsRepository = new Lazy<ISettingsRepository>(() => new SettingsRepository());
        }

        public IUserRepository UserRepository => _lazyUserRepository.Value;
        public IPasswordRepository PasswordRepository => _lazyPasswordRepository.Value;
        public ISettingsRepository SettingsRepository => _lazySettingsRepository.Value;
        public IRepositoryBase<T>? GetRepository<T>() where T : class
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(User):
                    return UserRepository as IRepositoryBase<T>;
                case Type t when t == typeof(Password):
                    return PasswordRepository as IRepositoryBase<T>;
                case Type t when t == typeof(Settings):
                    return SettingsRepository as IRepositoryBase<T>;
                default:
                    break;
            }
            return null;
        }

        public Task<int> SaveChangesAsync()
        {
            // empty operation for MAUI
            return Task.FromResult(0);
        }
    }
}
