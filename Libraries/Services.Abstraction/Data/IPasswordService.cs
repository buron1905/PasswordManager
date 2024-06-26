﻿using Models;
using Models.DTOs;

namespace Services.Abstraction.Data
{
    public interface IPasswordService : IDataServiceBase<Password>
    {
        Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId);
        Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId);
        Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO);
        Task<PasswordDTO> UpdateAsync(Guid userId, PasswordDTO passwordDTO);
        Task DeleteAsync(Guid userId, PasswordDTO passwordDTO);

        // encryption
        Task<PasswordDTO> EncryptPasswordAsync(PasswordDTO passwordDTO, string cipherKey, bool encryptOnlyNames = false);
        Task<PasswordDTO> DecryptPasswordAsync(PasswordDTO passwordDTO, string cipherKey, bool decryptOnlyNames = false);
    }
}
