using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IPasswordService> _lazyPasswordService;
        private readonly Lazy<ISettingsService> _lazySettingsService;

        public ServiceWrapper(IRepositoryWrapper repositoryWrapper)
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
