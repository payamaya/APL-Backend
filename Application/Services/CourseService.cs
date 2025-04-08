/*using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class CourseService : ICourseService
    {

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            // Temporary mock for testing
            return await Task.FromResult(new List<CourseDto>
            {
                new CourseDto { Id = 1, Title = "Sample Course", Description = "Intro to Full Stack" }
            });
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            // Temporary mock
            return await Task.FromResult(new CourseDto { Id = id, Title = "Course " + id, Description = "Details..." });
        }

        public async Task<CourseDto> CreateCourseAsync(CourseDto dto)
        {
            // Temporary mock
            dto.Id = new Random().Next(100); // Simulate DB-generated ID
            return await Task.FromResult(dto);
        }

        public async Task<CourseDto> UpdateCourseAsync(CourseDto dto)
        {
            return await Task.FromResult(dto); // Simulate update
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await Task.FromResult(true); // Simulate delete
        }
    }
}
*/