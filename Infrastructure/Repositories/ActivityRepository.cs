using Application.DTOs;
using Application.Interfaces;          // contains IActivityService & IFileStorage
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;       // For IFormFile
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;                     // For Enumerable.Empty
using System.Threading.Tasks;
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
            IFileStorage storage

        )
        {
            _context = context;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId)
        {
            var activities = await _context.Activities
                .Include(a => a.Attachments)       // load attachments
                .Where(a => a.ModuleId == moduleId)
                .ToListAsync();
            return activities.Select(a =>
            {
                var dto = _mapper.Map<ActivityDto>(a);
                dto.Attachments = a.Attachments     // **NEW** map attachments to DTOs
                    .Select(att => new AttachmentDto
                    {
                        Id = att.Id,
                        FileName = att.FileName,
                        Url = $"/api/course/module/{a.ModuleId}/activity/{a.Id}/attachments/{att.Id}"
                    })
                    .ToList();
                return dto;
            });
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId)
        {
            var activity = await _context.Activities
                .Include(a => a.Attachments)
                .FirstOrDefaultAsync(m => m.Id == activityId && m.ModuleId == moduleId);
            if (activity == null) return null;

            var dto = _mapper.Map<ActivityDto>(activity);
            dto.Attachments = activity.Attachments // **NEW**
                .Select(att => new AttachmentDto
                {
                    Id = att.Id,
                    FileName = att.FileName,
                    Url = $"/api/course/module/{activity.ModuleId}/activity/{activity.Id}/attachments/{att.Id}"
                })
                .ToList();
            return dto;
        }

        public async Task<ActivityDto> CreateActivityAsync(ActivityDto dto)
        {
            // Verify the module exists
            var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
            if (!moduleExists)
                throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");

            // Map and save the Activity
            var activity = _mapper.Map<Activity>(dto);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            // Persist each uploaded file as an ActivityAttachment
            var urls = new List<string>();
            foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
            {
                //using var ms = new MemoryStream();
                //await file.CopyToAsync(ms);

                //var attachment = new ActivityAttachment
                //{
                //    ActivityId = activity.Id,
                //    FileName = file.FileName,
                //    ContentType = file.ContentType ?? "application/octet-stream",
                //    Data = ms.ToArray(),
                //};
                //_context.ActivityAttachments.Add(attachment);
                var url = await _storage.SaveAsync(file);   // e.g. "/uploads/abc.pdf"
                urls.Add(url);
            }
            // 3) Store URLs on the entity and save again
            activity.AttachmentUrls = urls;
            await _context.SaveChangesAsync();

            // 4) Return DTO with URLs
            var result = _mapper.Map<ActivityDto>(activity);
            result.AttachmentUrls = urls;
            return result;
            //await _context.SaveChangesAsync();

            // Map back to DTO with attachments
            //var created = _mapper.Map<ActivityDto>(activity);
            //created.Attachments = activity.Attachments  // **NEW**
            //    .Select(att => new AttachmentDto
            //    {
            //        Id = att.Id,
            //        FileName = att.FileName,
            //        Url = $"/api/course/module/{activity.ModuleId}/activity/{activity.Id}/attachments/{att.Id}"
            //    })
            //    .ToList();
            //return created;                             // **CHANGED** now includes attachments
        }


        //public async Task<ActivityDto> UpdateActivityAsync(ActivityDto dto, string urls)
        //{
        //    //// Load existing activity + attachments
        //    //var activity = await _context.Activities
        //    //    .Include(a => a.Attachments)
        //    //    .FirstOrDefaultAsync(a => a.Id == dto.Id)
        //    //    ?? throw new Exception("Activity not found");
        //    var activity = await _context.Activities.FindAsync(dto.Id)
        //                   ?? throw new KeyNotFoundException();

        //    // Verify module exists if changed
        //    if (activity.ModuleId != dto.ModuleId)
        //    {
        //        var moduleExists = await _context.Modules.AnyAsync(c => c.Id == dto.ModuleId);
        //        if (!moduleExists)
        //            throw new InvalidOperationException($"Module with ID {dto.ModuleId} does not exist");
        //    }

        //    // Map updated fields
        //    _mapper.Map(dto, activity);
        //    await _context.SaveChangesAsync();
        //    var urls = activity.AttachmentUrls.ToList();
        //    foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
        //    {
        //        var url = await _storage.SaveAsync(file);
        //        urls.Add(url);
        //    }

        //    activity.AttachmentUrls = urls;
        //    await _context.SaveChangesAsync();

        //    var result = _mapper.Map<ActivityDto>(activity);
        //    result.AttachmentUrls = urls;
        //    return result;
        //}


        //    // Persist any newly uploaded files
        //    foreach (var file in dto.Files ?? Enumerable.Empty<IFormFile>())
        //    {
        //        using var ms = new MemoryStream();
        //        await file.CopyToAsync(ms);
        //        var attachment = new ActivityAttachment
        //        {                       // **NEW**
        //            ActivityId = activity.Id,
        //            FileName = file.FileName,
        //            ContentType = file.ContentType ?? "application/octet-stream",
        //            Data = ms.ToArray()
        //        };
        //        _context.ActivityAttachments.Add(attachment);
        //    }
        //    // Map back to DTO with attachments
        //    var updated         = _mapper.Map<ActivityDto>(activity);
        //    updated.Attachments = activity.Attachments
        //        .Select(att => new AttachmentDto {
        //            Id       = att.Id,
        //            FileName = att.FileName,
        //            Url      = $"/api/course/module/{activity.ModuleId}/activity/{activity.Id}/attachments/{att.Id}"
        //        })
        //        .ToList();
        //    return updated;
        //}

        public async Task<bool> DeleteActivityAsync(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            // Remove all attachments
            var attachments = await _context.ActivityAttachments
                .Where(att => att.ActivityId == id)
                .ToListAsync();
            _context.ActivityAttachments.RemoveRange(attachments);

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
