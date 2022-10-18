using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(Guid userId);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> CreateAsync(RegisterRequestDTO registerDTO);
        Task UpdateAsync(Guid userId, UpdateUserDTO updateUserDTO);
        Task DeleteAsync(Guid userId);
    }
}
