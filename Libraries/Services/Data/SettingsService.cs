using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data
{
    public class SettingsService : DataServiceBase<Settings>, ISettingsService
    {
        public SettingsService(IRepositoryWrapper repositoryWrapper)
            : base(repositoryWrapper)
        {
        }

        public async Task<SettingsDTO> GetSettingsByUser(Guid userId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _repositoryWrapper.SettingsRepository.GetSettingsByUser(userId);

            if (settings is null)
            {
                throw new SettingsNotFoundException();
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            var settingsDTO = settings.Adapt<SettingsDTO>();

            return settingsDTO;
        }

        // Maybe use FindByCondition
        public async Task<SettingsDTO> GetByIdAsync(Guid userId, Guid settingsId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _repositoryWrapper.SettingsRepository.GetByIdAsync(settingsId);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsId);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            var settingsDTO = settings.Adapt<SettingsDTO>();

            return settingsDTO;
        }

        public async Task<SettingsDTO> CreateAsync(Guid userId, SettingsDTO settingsDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = settingsDTO.Adapt<Settings>();

            settings.UserId = user.Id;

            _repositoryWrapper.SettingsRepository.Create(settings);

            await _repositoryWrapper.SaveChangesAsync();

            return settings.Adapt<SettingsDTO>();
        }

        public async Task UpdateAsync(Guid userId, SettingsDTO settingsDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _repositoryWrapper.SettingsRepository.GetByIdAsync(settingsDTO.Id);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsDTO.Id);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            // TODO: Does this work or setting.Value = settingsDTO.Value
            settings = settingsDTO.Adapt<Settings>();


            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid settingsId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var settings = await _repositoryWrapper.SettingsRepository.GetByIdAsync(settingsId);

            if (settings is null)
            {
                throw new SettingsNotFoundException(settingsId);
            }

            if (settings.UserId != user.Id)
            {
                throw new SettingsDoesNotBelongToUserException(user.Id, settings.Id);
            }

            _repositoryWrapper.SettingsRepository.Delete(settings);

            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
