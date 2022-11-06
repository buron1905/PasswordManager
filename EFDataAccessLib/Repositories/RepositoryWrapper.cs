using Models;
using Services.Abstraction.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _dataContext;
        private readonly Lazy<IUserRepository> _lazyUserRepository;
        private readonly Lazy<IPasswordRepository> _lazyPasswordRepository;
        private readonly Lazy<ISettingsRepository> _lazySettingsRepository;

        public RepositoryWrapper(DataContext dataContext)
        {
            _dataContext = dataContext;
            _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(dataContext));
            _lazyPasswordRepository = new Lazy<IPasswordRepository>(() => new PasswordRepository(dataContext));
            _lazySettingsRepository = new Lazy<ISettingsRepository>(() => new SettingsRepository(dataContext));
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
            return _dataContext.SaveChangesAsync();
        }
    }
}
