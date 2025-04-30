/*using Application.DTOs;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

using Activity = Domain.Entities.Activity;

namespace Infrastructure.Repositories
{
    public class ActivityRepositort : IActivityRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityRepositort(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId)
        {
            var activities = await _context.Activities
                .Where(a => a.ModuleId == moduleId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId)
        {
            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.Id == activityId && m.ModuleId == moduleId);
            return activity == null ? null : _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> CreateActivityAsync(ActivityDto dto)
        {
            // Verify the course exists first
            var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
            if (!moduleExists)
            {
                throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");
            }

            var activity = _mapper.Map<Activity>(dto);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> UpdateActivityAsync(ActivityDto dto)
        {
            var activity = await _context.Activities.FindAsync(dto.Id);
            if (activity == null) throw new Exception("Activity not found");

            // Verify the module exists if ModuleId is being updated
            if (activity.ModuleId != dto.ModuleId)
            {
                var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
                if (!moduleExists)
                {
                    throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");
                }
            }

            _mapper.Map(dto, activity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<bool> DeleteActivityAsync(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ModuleExists(Guid moduleId)
            => await _context.Modules.AnyAsync(m => m.Id == moduleId);
    }
}*/

using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
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
    }
}
