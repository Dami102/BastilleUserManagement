using BastilleIUserLibrary.Domain.Model;
using BastilleUserService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BastilleUserLibrary.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<User> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
