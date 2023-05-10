using BastilleUserService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BastilleUserLibrary.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context { get; set; }
        private UserRepository _userRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
