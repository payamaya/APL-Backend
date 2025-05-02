using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Domain.Entities.File>> GetAllAsync();
        Task<Domain.Entities.File?> GetByIdAsync(Guid id);
        Task AddAsync(Domain.Entities.File course);
        Task UpdateAsync(Domain.Entities.File course);
        Task DeleteAsync(Domain.Entities.File course);
    }
}
