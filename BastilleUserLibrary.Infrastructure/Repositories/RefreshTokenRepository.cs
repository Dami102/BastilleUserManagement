using BastilleUserService.Core.DTOs.Request;
using BastilleUserService.Core.Interfaces;

namespace BastilleUserLibrary.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public Task Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid().ToString();
            _refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetRefreshToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(x => x.Token == token);

            return Task.FromResult(refreshToken);
        }

        public Task DeleteAll(string userEmail)
        {
            _refreshTokens.RemoveAll(x => x.UserEmail == userEmail);
            return Task.CompletedTask;
        }
    }
}
