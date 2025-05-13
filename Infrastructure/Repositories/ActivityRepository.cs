

using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IBaseRepository<Activity>, IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<Activity?> GetByIdAsync(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Activity activity)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ModuleExists(Guid moduleId)
        {
            return await _context.Modules.AnyAsync(m => m.Id == moduleId);
        }

        public Task<IEnumerable<Activity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
