using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindByEmailAsync(string email);
        void Delete(User user);
    }

}
