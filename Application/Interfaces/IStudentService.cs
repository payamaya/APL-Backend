using Application.DTOs;

namespace Application.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDto> CreateStudentAsync(StudentDto dto);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(Guid id);
        Task<StudentDto> UpdateStudentAsync(StudentDto dto);
        Task<bool> DeleteStudentAsync(Guid id);
    }
}
