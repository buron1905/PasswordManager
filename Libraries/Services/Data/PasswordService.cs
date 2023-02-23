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
        public PasswordService(IRepositoryWrapper repositoryWrapper)
            : base(repositoryWrapper)
        {
        }

        public async Task<IEnumerable<PasswordDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var passwords = await _repositoryWrapper.PasswordRepository.GetAllByUserIdAsync(userId);

            //TODO: PasswordEncrypted to PasswordDecrypted - now in Controller

            var passwordsDTO = passwords.Adapt<IEnumerable<PasswordDTO>>();

            return passwordsDTO;
        }

        // Maybe use FindByCondition
        public async Task<PasswordDTO> GetByIdAsync(Guid userId, Guid passwordId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordId);

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
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = passwordDTO.Adapt<Password>();

            password.UserId = user.Id;
            password.User = user;
            //TODO: use encryption service - now in controller
            //password.PasswordEncrypted = password.PasswordDecrypted;

            _repositoryWrapper.PasswordRepository.Create(password);

            await _repositoryWrapper.SaveChangesAsync();

            return password.Adapt<PasswordDTO>();
        }

        public async Task<PasswordDTO> UpdateAsync(Guid userId, PasswordDTO passwordDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordDTO.Id);

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

            await _repositoryWrapper.SaveChangesAsync();

            return password.Adapt<PasswordDTO>();
        }

        public async Task DeleteAsync(Guid userId, Guid passwordId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var password = await _repositoryWrapper.PasswordRepository.GetByIdAsync(passwordId);

            if (password is null)
            {
                throw new PasswordNotFoundException(passwordId);
            }

            if (password.UserId != user.Id)
            {
                throw new PasswordDoesNotBelongToUserException(user.Id, password.Id);
            }

            _repositoryWrapper.PasswordRepository.Delete(password);

            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
