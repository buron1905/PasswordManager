﻿using Models;
using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface IUserService : IDataServiceBase<User>
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(Guid userId);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> CreateAsync(UserDTO userDTO);
        Task<UserDTO> UpdateAsync(UserDTO userDTO);
        Task DeleteAsync(Guid userId);
    }
}
