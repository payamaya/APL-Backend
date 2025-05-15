using Domain.Entities;
using Domain.Entities.Base;
using Domain.Interfaces;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IActivityRepository : IBaseRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid moduleId);
        Task<Activity?> GetByIdAsync(Guid moduleId, Guid activityId);
        Task<bool> ModuleExists(Guid moduleId);
    }
}
