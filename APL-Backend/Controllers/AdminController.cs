using Application.DTOs;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        public UserService Get_userService()
        {
            return _userService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto, UserService _userService)
        {
            var user = await _userService.CreateAsync(dto);
            return Ok(user);
        }

        [HttpPost("assign-course")]

        public async Task<IActionResult> AssignToCourse([FromBody] AssignUserToCourseDto dto)
        {
            var _userService.AssignUserToCourseAsync(dto);
            return Ok("Assigned Successfully!");
        }

    }
}
