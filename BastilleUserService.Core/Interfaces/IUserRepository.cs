using BastilleIUserLibrary.Domain.Model;

namespace BastilleUserService.Core.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllUsers();
        Task<User> GetUserById(string id);
    }
}