using Application.DTOs;
using Application.Interfaces;          // contains IActivityService & IFileStorage
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;       // For IFormFile
using Microsoft.EntityFrameworkCore;
using Activity = Domain.Entities.Activity;

namespace Infrastructure.Repositories
{
    public class ActivityService : IActivityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorage _storage;   // **NEW**

        // **CHANGED**: inject IFileStorage
        public ActivityService(
            AppDbContext context,
            IMapper mapper,
            IFileStorage storage              // **NEW**
        )
        {
            _context = context;
            _mapper = mapper;
            _storage = storage;                 // **NEW**
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
            if (activity == null) return null;
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> CreateActivityAsync(ActivityDto dto)
        {
            // Verify module exists
            if (!await _context.Modules.AnyAsync(m => m.Id == dto.ModuleId))
                throw new InvalidOperationException($"Module {dto.ModuleId} not found");

            // Map & save to get Id
            var activity = _mapper.Map<Activity>(dto);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            // **NEW**: start with any URLs client passed in
            var urls = dto.AttachmentUrls?.ToList() ?? new List<string>();

            // **NEW**: save each uploaded file and collect its URL
            foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
            {
                var savedUrl = await _storage.SaveAsync(file);
                urls.Add(savedUrl);
            }

            // **NEW**: persist merged URL list back into the JSON column
            activity.AttachmentUrls = urls;
            await _context.SaveChangesAsync();

            // **NEW**: map back out and include the full URL list in the DTO
            var result = _mapper.Map<ActivityDto>(activity);
            result.AttachmentUrls = urls;
            return result;
        }

        // **NEW**: fully-implemented Update merges incoming URLs + files
        public async Task<ActivityDto> UpdateActivityAsync(ActivityDto dto)
        {
            var activity = await _context.Activities.FindAsync(dto.Id)
                           ?? throw new InvalidOperationException("Activity not found");

            // (optional) verify dto.ModuleId exists, etc.

            // map scalar fields
            _mapper.Map(dto, activity);
            await _context.SaveChangesAsync();

            // **NEW**: start from existing URLs
            var urls = activity.AttachmentUrls?.ToList() ?? new List<string>();

            // **NEW**: add any URLs the client passed in
            if (dto.AttachmentUrls != null)
                urls.AddRange(dto.AttachmentUrls);

            // **NEW**: process any newly uploaded files
            foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
            {
                var savedUrl = await _storage.SaveAsync(file);
                urls.Add(savedUrl);
            }

            // **NEW**: persist merged list and round-trip back to DTO
            activity.AttachmentUrls = urls;
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ActivityDto>(activity);
            result.AttachmentUrls = urls;
            return result;
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