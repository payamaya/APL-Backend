using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _TeacherService;

        public TeacherController(ITeacherService TeacherService)
        {
            _TeacherService = TeacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _TeacherService.GetAllTeachersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _TeacherService.GetTeacherByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherDto dto)
        {

            // If validation passes, create the Teacher
            var createdTeacher = await _TeacherService.CreateTeacherAsync(dto);
            return Ok(createdTeacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TeacherDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            // Get the current time in Central European Time (CET)
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime currentCET = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);

            return Ok(await _TeacherService.UpdateTeacherAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _TeacherService.DeleteTeacherAsync(id));
    }
}
