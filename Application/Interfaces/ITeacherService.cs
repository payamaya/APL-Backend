using Application.DTOs;


namespace Application.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto?> GetTeacherByIdAsync(Guid id);
        Task<TeacherDto> CreateTeacherAsync(TeacherDto dto);
        Task<TeacherDto> UpdateTeacherAsync(TeacherDto dto);
        Task<bool> DeleteTeacherAsync(Guid id);
    }
}
