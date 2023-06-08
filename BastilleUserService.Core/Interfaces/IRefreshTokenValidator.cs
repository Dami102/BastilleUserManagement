namespace BastilleUserService.Core.Interfaces
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);
    }
}