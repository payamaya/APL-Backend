using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(Guid id);
        Task AddAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task DeleteAsync(Teacher teacher);
        Task SaveChangesAsync();
    }
}
