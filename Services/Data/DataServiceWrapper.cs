using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data
{
    public class DataServiceWrapper : IDataServiceWrapper
    {
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IPasswordService> _lazyPasswordService;
        private readonly Lazy<ISettingsService> _lazySettingsService;
        private readonly Lazy<IRefreshTokenService> _lazyRefreshTokenService;

        public DataServiceWrapper(IRepositoryWrapper repositoryWrapper)
        {
            _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryWrapper));
            _lazyPasswordService = new Lazy<IPasswordService>(() => new PasswordService(repositoryWrapper));
            _lazySettingsService = new Lazy<ISettingsService>(() => new SettingsService(repositoryWrapper));
            _lazyRefreshTokenService = new Lazy<IRefreshTokenService>(() => new RefreshTokenService(repositoryWrapper));
        }

        public IUserService UserService => _lazyUserService.Value;
        public IPasswordService PasswordService => _lazyPasswordService.Value;
        public ISettingsService SettingsService => _lazySettingsService.Value;
        public IRefreshTokenService RefreshTokenService => _lazyRefreshTokenService.Value;
    }
}
