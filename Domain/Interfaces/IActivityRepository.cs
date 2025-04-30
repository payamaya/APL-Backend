using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllAsync(Guid moduleId);
        Task<Activity?> GetByIdAsync(Guid moduleId, Guid activityId);
        Task<Activity?> GetByIdAsync(Guid id); // for update/delete
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(Activity activity);
        Task<bool> ModuleExists(Guid moduleId);
    }
}
