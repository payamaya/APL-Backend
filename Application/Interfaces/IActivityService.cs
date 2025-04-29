using Application.DTOs;
using Microsoft.AspNetCore.Http;
namespace Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync(Guid moduleId);
        Task<ActivityDto?> GetActivityByIdAsync(Guid moduleId, Guid activityId);
        Task<ActivityDto> CreateActivityAsync(ActivityDto dto);
        //Task<ActivityDto> UpdateActivityAsync(ActivityDto dto);
        Task<bool> DeleteActivityAsync(Guid activityId);
    }

    public interface IFileStorage // Abstraction for saving uploaded files
    {
        /// <summary>
        /// Persist the given IFormFile (e.g. to disk, S3, blob storage, etc.) 
        /// and return a public URL or path to the saved file.
        /// </summary>
        Task<string> SaveAsync(IFormFile file);
    }
}