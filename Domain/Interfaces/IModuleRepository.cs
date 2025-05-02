using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllAsync(Guid courseId);
        Task<Module?> GetByIdAsync(Guid courseId, Guid moduleId);
        Task AddAsync(Module module);
        Task UpdateAsync(Module module);
        Task DeleteAsync(Module module);
    }
}
