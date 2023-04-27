using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;

namespace Services.Data
{
    public class SettingsService : DataServiceBase<Settings>, ISettingsService
    {
        ISettingsRepository _settingsRepository;
        IUserRepository _userRepository;

        public SettingsService(ISettingsRepository settingsRepository, IUserRepository userRepository)
            : base(settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _userRepository = userRepository;
        }

        public async Task<SettingsDTO> GetSettingsByUser(Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _settingsRepository.GetSettingsByUser(userId);

            if (settings is null)
            {
                throw new SettingsNotFoundException();
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            return settings.Adapt<SettingsDTO>();
        }

        // Maybe use FindByCondition
        public async Task<SettingsDTO> GetByIdAsync(Guid userId, Guid settingsId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _settingsRepository.FindByIdAsync(settingsId);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsId);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            return settings.Adapt<SettingsDTO>(); ;
        }

        public async Task<SettingsDTO> CreateAsync(Guid userId, SettingsDTO settingsDTO)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = settingsDTO.Adapt<Settings>();

            settings.UserId = user.Id;

            await _settingsRepository.Create(settings);

            return settings.Adapt<SettingsDTO>();
        }

        public async Task<SettingsDTO> UpdateAsync(Guid userId, SettingsDTO settingsDTO)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _settingsRepository.FindByIdAsync(settingsDTO.Id);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsDTO.Id);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            // TODO: check update logic
            settings.UserId = user.Id;
            settings.SavePassword = settingsDTO.SavePassword;

            return settings.Adapt<SettingsDTO>();
        }

        public async Task DeleteAsync(Guid userId, Guid settingsId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _settingsRepository.FindByIdAsync(settingsId);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsId);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            await _settingsRepository.Delete(settings);
        }
    }
}
