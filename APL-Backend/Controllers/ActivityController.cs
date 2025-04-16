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

            dto.ModuleId = moduleId; // Ensure course association remains
            return Ok(await _activityService.UpdateActivityAsync(dto));
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(Guid moduleId, Guid activityId) =>
            Ok(await _activityService.DeleteActivityAsync(activityId));
    }
}