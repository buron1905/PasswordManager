﻿using Models;
using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface IUserService : IDataServiceBase<User>
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(Guid userId);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> CreateAsync(RegisterRequestDTO registerDTO);
        Task UpdateAsync(Guid userId, UpdateUserDTO updateUserDTO);
        Task DeleteAsync(Guid userId);
    }
}