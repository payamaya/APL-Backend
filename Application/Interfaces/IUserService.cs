using Application.DTOs;
using Application.DTOs.Base;
using Application.Interfaces.Base;


namespace Application.Interfaces
{
    public interface IUserService: ICrudService<UserDto>
    {
        // Task<IEnumerable<UserDto>> GetAllUsersAsync();
        // Task<UserDto?> GetUserByIdAsync(Guid id);
        // Task<UserDto> CreateUserAsync(UserDto dto);
        // Task<UserDto> UpdateUserAsync(UserDto dto);
        // Task<bool> DeleteUserAsync(Guid id);
        Task AssignUserToCourseAsync(AssignUserToCourseDto dto);
        Task RemoveUserFromCourseAsync(AssignUserToCourseDto dto);
    }

}
