﻿using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;

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
            var user = await _repositoryWrapper.UserRepository.FindSingleOrDefaultByCondition(user => user.EmailAddress.Equals(email));

            if (user is null)
                throw new UserNotFoundException(email);

            var userDTO = user.Adapt<UserDTO>();
            return userDTO;
        }

        public async Task<UserDTO> CreateAsync(RegisterRequestDTO registerDTO)
        {
            var user = registerDTO.Adapt<User>();
            user.EmailConfirmationToken = Guid.NewGuid().ToString();

            _repositoryWrapper.UserRepository.Create(user);

            await _repositoryWrapper.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> UpdateAsync(Guid userId, UserDTO userDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            // TODO: Check update logic
            user.EmailAddress = userDTO.EmailAddress;
            user.PasswordHASH = userDTO.PasswordHASH;
            user.TwoFactorEnabled = userDTO.TwoFactorEnabled;
            user.TwoFactorSecret = userDTO.TwoFactorSecret;
            user.EmailConfirmed = userDTO.EmailConfirmed;
            user.EmailConfirmationToken = userDTO.EmailConfirmationToken;

            await _repositoryWrapper.SaveChangesAsync();

            return user.Adapt<UserDTO>();
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
