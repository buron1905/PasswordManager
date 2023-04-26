using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;

namespace Services.Data
{
    public class PasswordService : DataServiceBase<Password>, IPasswordService
    {
        IPasswordRepository _passwordRepository;
        IUserRepository _userRepository;

        public PasswordService(IPasswordRepository passwordRepository, IUserRepository userRepository)
            : base(passwordRepository)
        {
            _passwordRepository = passwordRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var passwords = await _passwordRepository.GetAllByUserIdAsync(userId);

            //TODO: PasswordEncrypted to PasswordDecrypted - now in Controller

            var passwordsDTO = passwords.Adapt<IEnumerable<PasswordDTO>>();

            return passwordsDTO;
        }

        // Maybe use FindByCondition
        public async Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _passwordRepository.GetByIdAsync(passwordId);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordId);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            var passwordDTO = password.Adapt<PasswordDTO>();

            return passwordDTO;
        }

        public async Task<PasswordDTO> CreateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = passwordDTO.Adapt<Password>();

            password.UserId = user.Id;
            //password.User = user;
            //TODO: use encryption service - now in controller
            //password.PasswordEncrypted = password.PasswordDecrypted;

            // This automatically fills the ID
            await _passwordRepository.Create(password);

            return password.Adapt<PasswordDTO>();
        }

        public async Task<PasswordDTO> UpdateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _passwordRepository.GetByIdAsync(passwordDTO.Id);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordDTO.Id);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            // TODO: Check update logic
            password.PasswordName = passwordDTO.PasswordName;
            password.UserName = passwordDTO.UserName;
            password.PasswordEncrypted = passwordDTO.PasswordEncrypted;
            password.PasswordDecrypted = passwordDTO.PasswordDecrypted;
            password.URL = passwordDTO.URL;
            password.Notes = passwordDTO.Notes;
            password.Favorite = passwordDTO.Favorite;
            password.UDT = passwordDTO.UDT;
            password.IDT = passwordDTO.IDT;

            await _passwordRepository.Update(password);

            return password.Adapt<PasswordDTO>();
        }

        public async Task DeleteAsync(Guid userId, Guid passwordId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _passwordRepository.GetByIdAsync(passwordId);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordId);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            await _passwordRepository.Delete(password);
        }
    }
}
