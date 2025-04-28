using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using DocumentFormat.OpenXml.InkML;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task AssignUserToCourseAsync(AssignUserToCourseDto dto);

        //public async Task<List<UserDto>> GetAllTeachersAsync()
        //{
        //    var teachers = await _context.Users
        //        .Where(u => u.Role == "Teacher") // or use u.IsTeacher == true if you have that
        //        .ToListAsync();

        //    return _mapper.Map<List<UserDto>>(teachers);
        //}


        //Task<List<UserDto>> GetAllUsersAsync();

    }
}
