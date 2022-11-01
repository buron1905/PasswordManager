using Mapster;
using Models;
using Models.DTOs;
using Persistance;
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
    public class UserService : DataServiceBase<User>, IUserService
    {
        public UserService(IRepositoryWrapper repositoryWrapper)
            : base(repositoryWrapper)
        {
        }

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

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var user = await _repositoryWrapper.UserRepository.FindSingleOrDefaultByCondition(user => user.Email.Equals(email));
            
            if (user is null)
                throw new UserNotFoundException(email);

            var userDTO = user.Adapt<UserDTO>();
            return userDTO;
        }

        public async Task<UserDTO> GetByRefreshTokenAsync(string token)
        {
            var user = await _repositoryWrapper.UserRepository.FindSingleOrDefaultByCondition(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                throw new AppException("Invalid token");
                //throw new UserNotFoundException();

            var userDTO = user.Adapt<UserDTO>();
            return userDTO;
        }

        public async Task<UserDTO> CreateAsync(RegisterRequestDTO registerDTO)
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
