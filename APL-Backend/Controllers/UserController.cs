using Application.DTOs;
using Application.DTOs.Base;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const int PAST_DATE_TOLERANCE_SECONDS = -30;
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _UserService.GetAllAsync());

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _UserService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            await _UserService.CreateAsync(dto); // Removed assignment to a variable since CreateUserAsync returns void
            return Ok("User created successfully."); // Added a success message to indicate the operation was completed
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _UserService.UpdateAsync(dto); // Fix: Removed assignment to a variable since UpdateUserAsync returns void
            return Ok("User updated successfully."); // Added a success message to indicate the operation was completed
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _UserService.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }


    }
}
