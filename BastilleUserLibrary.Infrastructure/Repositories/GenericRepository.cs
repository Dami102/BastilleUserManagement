using BastilleUserService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BastilleUserLibrary.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(ApplicationDbContext context)
        {
            _context=context;
            _db = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task AddRangeAsync(T entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public void DeleteRange(T entities)
        {
            _db.RemoveRange(entities);
        }

        /*public void FIndById(string id)
        {
            _db.f
        }*/

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
