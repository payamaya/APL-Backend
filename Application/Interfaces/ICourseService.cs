using Application.DTOs;
using Application.DTOs.Base;

namespace Application.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDto> CreateCourseAsync(CourseDto dto);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(Guid id);
        Task<CourseDto> UpdateCourseAsync(CourseDto dto);
        Task<bool> DeleteCourseAsync(Guid id);

    }
}
