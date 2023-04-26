using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;

namespace Services.Data
{
    public class UserService : DataServiceBase<User>, IUserService
    {
        IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var usersDTO = users.Adapt<IEnumerable<UserDTO>>();

            return usersDTO;
        }

        public async Task<UserDTO> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var userDTO = user.Adapt<UserDTO>();

            return userDTO;
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var user = await _userRepository.FindSingleOrDefaultByCondition(user => user.EmailAddress.Equals(email));

            if (user is null)
                throw new UserNotFoundException(email);

            var userDTO = user.Adapt<UserDTO>();
            return userDTO;
        }

        public async Task<UserDTO> CreateAsync(UserDTO userDTO)
        {
            var user = userDTO.Adapt<User>();

            await _userRepository.Create(user);

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> UpdateAsync(UserDTO userDTO)
        {
            var user = await _userRepository.GetByIdAsync(userDTO.Id);

            if (user is null)
                throw new UserNotFoundException(userDTO.EmailAddress);

            // TODO: Check update logic
            user.EmailAddress = userDTO.EmailAddress;
            user.PasswordHASH = userDTO.PasswordHASH;
            user.TwoFactorEnabled = userDTO.TwoFactorEnabled;
            user.TwoFactorSecret = userDTO.TwoFactorSecret;
            user.EmailConfirmed = userDTO.EmailConfirmed;
            user.EmailConfirmationToken = userDTO.EmailConfirmationToken;

            await _userRepository.Update(user);

            return user.Adapt<UserDTO>();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            await _userRepository.Delete(user);
        }
    }
}
