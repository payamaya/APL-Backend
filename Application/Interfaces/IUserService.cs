using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(UserDto dto);
        Task<UserDto> UpdateUserAsync(UserDto dto);
        Task<bool> DeleteUserAsync(Guid id);
        Task AssignUserToCourseAsync(AssignUserToCourseDto dto);
        Task RemoveUserFromCourseAsync(AssignUserToCourseDto dto);
    }

}
