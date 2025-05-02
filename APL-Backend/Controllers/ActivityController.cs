using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;


namespace APL_Backend.Controllers
{
    [Route("api/course/module/{moduleId}/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private const int PAST_DATE_TOLERANCE_SECONDS = -30;
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid moduleId) =>
            Ok(await _activityService.GetAllActivitiesAsync(moduleId));

        [HttpGet("{activityId}")]
        public async Task<IActionResult> Get(Guid moduleId, Guid activityId)
        {
            var result = await _activityService.GetActivityByIdAsync(moduleId, activityId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid moduleId, [FromBody] ActivityDto dto)
        {
            if (!Enum.IsDefined(typeof(ActivityType), dto.ActivityType))
                return BadRequest("Invalid activity type.");

            if (dto.ActivityType == ActivityType.Assignment && dto.EndDate == null)   
            {
                throw new Exception("Assignments must have a due date.");
            }

            // Validate the StartDate
            var utcNow = DateTime.UtcNow;
            if (dto.StartDate < utcNow.AddSeconds(PAST_DATE_TOLERANCE_SECONDS))
            {
                return BadRequest("Start date cannot be in the past.");
            }

            // Validate that EndDate (if provided) is after StartDate
            if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
            {
                return BadRequest("End date must be after start date.");
            }

            try
            {
                dto.ModuleId = moduleId; // Ensure association
                var createdActivity = await _activityService.CreateActivityAsync(dto);
                return CreatedAtAction(
                    nameof(Get),
                    new { moduleId, activityId = createdActivity.Id },
                    createdActivity
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> Update(Guid moduleId, Guid activityId, [FromBody] ActivityDto dto)
        {
            if (activityId != dto.Id) return BadRequest("ID mismatch");

            if (!Enum.IsDefined(typeof(ActivityType), dto.ActivityType))
                return BadRequest("Invalid activity type.");
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

            dto.ModuleId = moduleId; // Ensure course association remains
            return Ok(await _activityService.UpdateActivityAsync(dto));
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(Guid moduleId, Guid activityId) =>
            Ok(await _activityService.DeleteActivityAsync(activityId));
    }
}