
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId);
        Task<ActivityDto> CreateActivityAsync(ActivityDto dto);
        Task<ActivityDto> UpdateActivityAsync(ActivityDto dto);
        Task<bool> DeleteActivityAsync(Guid activityId);
    }
}
