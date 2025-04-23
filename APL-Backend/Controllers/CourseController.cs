using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetAll() => Ok(await _courseService.GetAllCoursesAsync());

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto dto)
        {

            // Validate the StartDate
            if (dto.StartDate < DateTime.UtcNow)
            {
                return BadRequest("Start date cannot be in the past.");
            }

            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                return BadRequest("End date must be after start date.");
            }

            // If validation passes, create the course
            var createdCourse = await _courseService.CreateCourseAsync(dto);
            return Ok(createdCourse);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            // Get the current time in Central European Time (CET)
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime currentCET = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);

            // Validate the StartDate
            if (dto.StartDate < currentCET)
            {
                return BadRequest("Start date cannot be in the past.");
            }

            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                return BadRequest("End date must be after start date.");
            }

            return Ok(await _courseService.UpdateCourseAsync(dto));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _courseService.DeleteCourseAsync(id));
    }
}
