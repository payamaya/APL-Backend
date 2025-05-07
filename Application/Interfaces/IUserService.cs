using Application.DTOs;


namespace Application.Interfaces
{
    public interface IUserService
    {
        Task AssignUserToCourseAsync(UserDto dto);
        Task<Guid> CreateUserAsync(UserDto dto);
        Task<Guid> RegisterAsync(UserDto dto);
    }

}
