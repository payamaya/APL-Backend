using Application.DTOs;
using Application.DTOs.Base;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private const int PAST_DATE_TOLERANCE_SECONDS = -30;
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _studentService.GetAllStudentsAsync());

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            // Validate the StartDate
            var createdStudent = await _studentService.CreateStudentAsync(dto);
            return Ok(createdStudent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var updatedStudent = await _studentService.UpdateStudentAsync(dto);
            return Ok(updatedStudent);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return result ? Ok() : NotFound();
        }


    }
}
