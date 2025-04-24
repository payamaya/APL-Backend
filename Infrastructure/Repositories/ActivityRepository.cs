using Application.DTOs;
using Application.Interfaces;          // contains IActivityService & IFileStorage
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;       // **NEW** for IFormFile
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;                     // **NEW** for Enumerable.Empty
using System.Threading.Tasks;
using Activity = Domain.Entities.Activity;

namespace Infrastructure.Repositories
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;   // **NEW**

        // **CHANGED**: inject IFileStorage
        public ActivityService(
            AppDbContext context,
            IMapper mapper,
            IFileStorage fileStorage             // **NEW**
        )
        {
            _context = context;
            _mapper = mapper;
            _fileStorage = fileStorage;           // **NEW**
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
            return activity == null
                ? null
                : _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> CreateActivityAsync(ActivityDto dto)
        {
            // Verify the module exists
            var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
            if (!moduleExists)
                throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");

            // Map basic fields
            var activity = _mapper.Map<Activity>(dto);

            // **NEW**: handle incoming file uploads
            foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
            {
                var url = await _fileStorage.SaveAsync(file);
                activity.AttachmentUrls.Add(url);
            }

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> UpdateActivityAsync(ActivityDto dto)
        {
            var activity = await _context.Activities.FindAsync(dto.Id);
            if (activity == null)
                throw new Exception("Activity not found");

            // Verify module exists if changed
            if (activity.ModuleId != dto.ModuleId)
            {
                var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
                if (!moduleExists)
                    throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");
            }

            // **CHANGED**: map incoming DTO into entity (excluding attachments)
            _mapper.Map(dto, activity);

            // **NEW**: process any newly uploaded files
            foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
            {
                var url = await _fileStorage.SaveAsync(file);
                activity.AttachmentUrls.Add(url);
            }

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
    }
}
