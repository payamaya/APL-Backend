using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity = Domain.Entities.Activity;

namespace Infrastructure.Repositories
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllModulesAsync(Guid moduleId)
        {
            var activities = await _context.Modules
                .Where(m => m.ModuleId == moduleId)
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
        public async Task<IEnumerable<ActivityDto>> GetAllActivityAsync(Guid moduleId)
        {
            var activities = await _context.Activities
                .Where(a => a.ModuleId == moduleId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ActivityDto>>(activities);
        }
    }
}