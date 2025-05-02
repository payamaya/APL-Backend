using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        //[Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetAll() => Ok(await _studentService.GetAllStudentsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDto dto)
        {
            // Validate the StartDate
            var utcNow = DateTime.UtcNow;
            if (dto.StartDate < utcNow.AddSeconds(-30))
            {
                return BadRequest("Start date cannot be in the past.");
            }
            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                return BadRequest("End date must be after start date.");
            }
            var createdStudent = await _studentService.CreateStudentAsync(dto);
            return Ok(createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var updatedStudent = await _studentService.UpdateStudentAsync(dto);
            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            return result ? Ok() : NotFound();
        }


    }
}
