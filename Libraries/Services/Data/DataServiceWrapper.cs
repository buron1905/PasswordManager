using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;

namespace Services.Data
{
    public class DataServiceWrapper : IDataServiceWrapper
    {
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IPasswordService> _lazyPasswordService;
        private readonly Lazy<ISettingsService> _lazySettingsService;

        public DataServiceWrapper(IRepositoryWrapper repositoryWrapper)
        {
            _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryWrapper));
            _lazyPasswordService = new Lazy<IPasswordService>(() => new PasswordService(repositoryWrapper));
            _lazySettingsService = new Lazy<ISettingsService>(() => new SettingsService(repositoryWrapper));
        }

        public IUserService UserService => _lazyUserService.Value;
        public IPasswordService PasswordService => _lazyPasswordService.Value;
        public ISettingsService SettingsService => _lazySettingsService.Value;
    }
}
