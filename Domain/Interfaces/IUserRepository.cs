using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> FindByEmailAsync(string email);
        Task SaveChangesAsync();
        void Delete(User user);
    }

}
