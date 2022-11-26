﻿using Models;
using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface IPasswordService : IDataServiceBase<Password>
    {
        Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId);
        Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId);
        Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO);
        Task UpdateAsync(Guid userId, PasswordDTO passwordDTO);
        Task DeleteAsync(Guid userId, Guid passwordId);
    }
}