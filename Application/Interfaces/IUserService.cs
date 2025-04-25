using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task AssignUserToCourseAsync(AssignUserToCourseDto dto);
        Task<List<UserDto>> GetAllUsersAsync();

    }
}
