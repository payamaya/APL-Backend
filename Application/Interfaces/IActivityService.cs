using Application.DTOs.Base;
using Application.Interfaces.Base;

namespace Application.Interfaces
{
    public interface IActivityService: ICrudService<ActivityDto>
    {
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId);
        // Task<ActivityDto> CreateActivityAsync(ActivityDto dto);
        // Task<ActivityDto> UpdateActivityAsync(ActivityDto dto);
        // Task<bool> DeleteActivityAsync(Guid activityId);
    }
}
