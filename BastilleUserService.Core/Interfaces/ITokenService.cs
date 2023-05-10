using BastilleIUserLibrary.Domain.Model;

namespace BastilleUserService.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        Task<string> GenerateToken(User user);
    }
}