using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }

}
