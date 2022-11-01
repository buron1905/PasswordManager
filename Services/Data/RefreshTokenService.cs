using Mapster;
using Models;
using Models.DTOs;
using Services.Abstraction.Data;
using Services.Abstraction.Data.Persistance;
using Services.Abstraction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data
{
    public class RefreshTokenService : DataServiceBase<RefreshToken>, IRefreshTokenService
    {
        public RefreshTokenService(IRepositoryWrapper repositoryWrapper)
            : base(repositoryWrapper)
        {
        }
        
        public async Task<IEnumerable<RefreshTokenDTO>> GetAllByUserIdAsync(Guid userId)
        {
            var refreshTokens = await _repositoryWrapper.RefreshTokenRepository.GetAllByUserIdAsync(userId);

            var refreshTokensDTO = refreshTokens.Adapt<IEnumerable<RefreshTokenDTO>>();

            return refreshTokensDTO;
        }
        
        public async Task<RefreshTokenDTO> GetByIdAsync(Guid userId, Guid refreshTokenId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var refreshToken = await _repositoryWrapper.RefreshTokenRepository.GetByIdAsync(refreshTokenId);

            if (refreshToken is null)
            {
                throw new RefreshTokenNotFoundException(refreshTokenId);
            }

            if (refreshToken.UserId != user.Id)
            {
                throw new AppException("Refresh token does not belong to user");
            }

            var refreshTokenDTO = refreshToken.Adapt<RefreshTokenDTO>();

            return refreshTokenDTO;
        }

        public async Task<RefreshTokenDTO> CreateAsync(Guid userId, RefreshTokenDTO refreshTokenDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var refreshToken = refreshTokenDTO.Adapt<RefreshToken>();

            refreshToken.UserId = user.Id;

            _repositoryWrapper.RefreshTokenRepository.Create(refreshToken);

            await _repositoryWrapper.SaveChangesAsync();

            return refreshToken.Adapt<RefreshTokenDTO>();
        }

        public async Task UpdateAsync(Guid userId, RefreshTokenDTO refreshTokenDTO)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var refreshToken = await _repositoryWrapper.RefreshTokenRepository.GetByIdAsync(refreshTokenDTO.Id);

            if (refreshToken is null)
            {
                throw new RefreshTokenNotFoundException(refreshTokenDTO.Id);
            }

            if (refreshToken.UserId != user.Id)
            {
                throw new AppException("Refresh token does not belong to user");
            }

            // TODO: Add update logic


            await _repositoryWrapper.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid refreshTokenId)
        {
            var user = await _repositoryWrapper.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var refreshToken = await _repositoryWrapper.RefreshTokenRepository.GetByIdAsync(refreshTokenId);

            if (refreshToken is null)
            {
                throw new RefreshTokenNotFoundException(refreshTokenId);
            }

            if (refreshToken.UserId != user.Id)
            {
                throw new AppException("Refresh token does not belong to user");
                //throw new RefreshTokenDoesNotBelongToUserException(user.Id, refreshToken.Id);
            }

            _repositoryWrapper.RefreshTokenRepository.Delete(refreshToken);

            await _repositoryWrapper.SaveChangesAsync();
        }
    }
}
