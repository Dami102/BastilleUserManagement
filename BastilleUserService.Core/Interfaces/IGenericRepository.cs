namespace BastilleUserService.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(T entities);
        void Delete(T entity);
        void DeleteRange(T entities);
        void Update(T entity);
    }
}