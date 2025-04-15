using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetAllActivityAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId);
        Task<ActivityDto> CreateActivityAsync(ActivityDto dto);
        Task<ActivityDto> UpdateActivityAsync(ActivityDto dto);
        Task<bool> DeleteActivityAsync(Guid activityId);
    }
}