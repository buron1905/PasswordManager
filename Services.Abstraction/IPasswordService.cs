using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IPasswordService
    {
        Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId);
        Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId);
        Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO);
        Task UpdateAsync(Guid userId, PasswordDTO passwordDTO);
        Task DeleteAsync(Guid userId, Guid passwordId);
    }
}
