using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLib.Repositories
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

        public Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
