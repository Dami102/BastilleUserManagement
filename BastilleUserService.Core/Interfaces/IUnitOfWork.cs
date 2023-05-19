using BastilleUserService.Core.Interfaces;

namespace BastilleUserService.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        void Dispose();
        Task SaveAsync();
    }
}