using BastilleUserService.Core.DTOs.Request;

namespace BastilleUserService.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task Create(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshToken(string token);
        Task DeleteAll(string userId);
    }
}