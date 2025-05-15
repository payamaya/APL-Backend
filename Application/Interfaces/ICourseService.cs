using Application.DTOs.Base;
using Application.Interfaces.Base;

namespace Application.Interfaces
{
    public interface ICourseService: ICrudService<CourseDto>
    {
        // Task<CourseDto> CreateCourseAsync(CourseDto dto);
        // Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        // Task<CourseDto?> GetCourseByIdAsync(Guid id);
        // Task<CourseDto> UpdateCourseAsync(CourseDto dto);
        // Task<bool> DeleteCourseAsync(Guid id);

    }
}
