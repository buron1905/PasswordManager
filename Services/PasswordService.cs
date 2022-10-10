using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.Abstraction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PasswordService(IRepositoryWrapper repositoryWrapper) => _repositoryWrapper = repositoryWrapper;

        public async Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var passwords = await _repositoryWrapper.PasswordRepository.GetAllByUserIdAsync(userId);

            var passwordsDTO = passwords.Adapt<IEnumerable<PasswordDTO>>();

            return passwordsDTO;
        }

        // Maybe use FindByCondition
        public async Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordId);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordId);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            var passwordDTO = password.Adapt<PasswordDTO>();

            return passwordDTO;
        }

        public async Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = passwordDTO.Adapt<Password>();

            password.UserId = user.Id;

            _repositoryWrapper.PasswordRepository.Create(password);

            await _repositoryWrapper.SaveChangesAsync();

            return password.Adapt<PasswordDTO>();
        }

        public async Task UpdateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordDTO.Id);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordDTO.Id);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            // TODO: Add update logic


            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid passwordId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordId);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordId);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            _repositoryWrapper.PasswordRepository.Delete(password);

            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
