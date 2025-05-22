using Application.DTOs;
using Application.DTOs.Base;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private const int PAST_DATE_TOLERANCE_SECONDS = -30;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        public CourseController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _courseService.GetAllCoursesAsync());

        //[Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _courseService.GetCourseByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseDto dto)
        {

            // Validate the StartDate
            var utcNow = DateTime.UtcNow;
            if (dto.StartDate < utcNow.AddSeconds(PAST_DATE_TOLERANCE_SECONDS))
            {
                throw new AppException("Start date cannot be in the past.");
            }

            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
               throw new AppException("End date must be after start date.");
            }

            // If validation passes, create the course
            var createdCourse = await _courseService.CreateCourseAsync(dto);
            return Ok(createdCourse);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            // Get the current time in Central European Time (CET)
            TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime currentCET = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);

            // Validate the StartDate
            if (dto.StartDate < currentCET)
            {
                throw new AppException("Start date cannot be in the past.");
            }

            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                throw new AppException("End date must be after start date.");
            }

            return Ok(await _courseService.UpdateCourseAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _courseService.DeleteCourseAsync(id));

        // ← START: enrollment endpoints
        [Authorize(Roles = "Admin")]
        [HttpPost("{courseId}/students/{studentId}")]
        public async Task<IActionResult> EnrollStudent(Guid courseId, Guid studentId)
        {
            await _userService.AssignUserToCourseAsync(new AssignUserToCourseDto   // ← added
            {
                UserId = studentId,
                CourseId = courseId
            });
            return Ok("Student enrolled successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}/students/{studentId}")]
        public async Task<IActionResult> UnenrollStudent(Guid courseId, Guid studentId)
        {
            await _userService.RemoveUserFromCourseAsync(new AssignUserToCourseDto  // ← added
            {
                UserId = studentId,
                CourseId = courseId
            });
            return Ok("Student unenrolled successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{courseId}/teachers/{teacherId}")]
        public async Task<IActionResult> EnrollTeacher(Guid courseId, Guid teacherId)
        {
            await _userService.AssignUserToCourseAsync(new AssignUserToCourseDto   // ← added
            {
                UserId = teacherId,
                CourseId = courseId
            });
            return Ok("Teacher enrolled successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{courseId}/teachers/{teacherId}")]
        public async Task<IActionResult> UnenrollTeacher(Guid courseId, Guid teacherId)
        {
            await _userService.RemoveUserFromCourseAsync(new AssignUserToCourseDto  // ← added
            {
                UserId = teacherId,
                CourseId = courseId
            });
            return Ok("Teacher unenrolled successfully.");
        }
        // ← END: enrollment endpoints
    }

}
