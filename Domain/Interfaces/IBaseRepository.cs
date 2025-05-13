

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync(); // optional if you're using Unit of Work
    }

}
