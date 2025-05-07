using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Authorize(Policy = "RequireAdmin")]    
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            await _userService.CreateUserAsync(dto); // Removed assignment to a variable
            return Ok("User created successfully!"); // Return a success message instead
        }

        [HttpPost("assign-course")]
        public async Task<IActionResult> AssignToCourse([FromBody] UserDto dto)
        {
            await _userService.AssignUserToCourseAsync(dto);
            return Ok("Assigned Successfully!");
        }

    }
}
