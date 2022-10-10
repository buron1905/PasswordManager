using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction;
using Services.Abstraction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {

        private readonly IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper) => _repositoryWrapper = repositoryWrapper;

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _repositoryWrapper.UserRepository.GetAllAsync();

            var usersDTO = users.Adapt<IEnumerable<UserDTO>>();

            return usersDTO;
        }

        public async Task<UserDTO> GetByIdAsync(Guid userId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var userDTO = user.Adapt<UserDTO>();

            return userDTO;
        }

        public async Task<UserDTO> CreateAsync(RegisterDTO registerDTO)
        {
            var user = registerDTO.Adapt<User>();

            _repositoryWrapper.UserRepository.Create(user);

            await _repositoryWrapper.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task UpdateAsync(Guid userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            // TODO: Add update logic
            user.Email = updateUserDTO.Email;

            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            _repositoryWrapper.UserRepository.Delete(user);

            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
