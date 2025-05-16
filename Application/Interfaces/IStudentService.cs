using Application.DTOs;
using Application.DTOs.Base;
using Application.Interfaces.Base;

namespace Application.Interfaces
{
    public interface IStudentService: ICrudService<StudentDto>
    {
        Task<StudentDto> CreateStudentAsync(StudentDto dto);
        // Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        // Task<StudentDto?> GetStudentByIdAsync(Guid id);
        // Task<StudentDto> UpdateStudentAsync(StudentDto dto);
        // Task<bool> DeleteStudentAsync(Guid id);
    }
}
