using Domain.Entities;
using Domain.Entities.Base;
using Domain.Interfaces;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindByEmailAsync(string email);
    }

}
