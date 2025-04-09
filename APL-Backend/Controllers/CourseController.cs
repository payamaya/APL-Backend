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
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] CourseDto dto) => Ok(await _courseService.CreateCourseAsync(dto));

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            return Ok(await _courseService.UpdateCourseAsync(dto));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _courseService.DeleteCourseAsync(id));
    }
}
