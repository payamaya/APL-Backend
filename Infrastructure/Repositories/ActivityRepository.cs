using Domain.Entities;
using Domain.Entities.Base;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Activity>> GetAllAsync(Guid moduleId)
        {
            return await _context.Activities
                .Where(a => a.ModuleId == moduleId)
                .ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(Guid moduleId, Guid activityId)
        {
            return await _context.Activities
                .FirstOrDefaultAsync(a => a.Id == activityId && a.ModuleId == moduleId);
        }

        public async Task<bool> ModuleExists(Guid moduleId)
        {
            return await _context.Modules.AnyAsync(m => m.Id == moduleId);
        }
    }
}
