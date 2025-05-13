using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IModuleRepository : IBaseRepository<Module>
    {
        Task<IEnumerable<Module>> GetAllAsync(Guid courseId);
        Task<Module?> GetByIdAsync(Guid courseId, Guid moduleId);
    }
}
