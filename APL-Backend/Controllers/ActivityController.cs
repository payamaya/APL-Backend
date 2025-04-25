using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Data;  // For AppDbContext
using Domain.Entities;      // For ActivityAttachment
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [ApiController]
    [Route("api/course/module/{moduleId}/[controller]")]
    [Consumes("application/json", "multipart/form-data")] // Enable file uploads
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly AppDbContext _context;           // **NEW**

        //public ActivityController(IActivityService activityS ervice)
        //{
        //    _activityService = activityService;
        //}

        // **CHANGED**: inject AppDbContext alongside IActivityService
        public ActivityController(
            IActivityService activityService,
            AppDbContext context           // **NEW**
        )
        {
            _activityService = activityService;
            _context = context;        // **NEW**
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid moduleId) =>
            Ok(await _activityService.GetAllActivitiesAsync(moduleId));

        [HttpGet("{activityId}", Name = "GetActivityById")]
        public async Task<IActionResult> Get(Guid moduleId, Guid activityId)
        {
            var result = await _activityService.GetActivityByIdAsync(moduleId, activityId);
            return result == null ? NotFound() : Ok(result);
        }

        // **1) NEW download endpoint**  
        [HttpGet("{activityId}/attachments/{attachmentId}")]  // **NEW**
        public async Task<IActionResult> DownloadAttachment(
            Guid moduleId,
            Guid activityId,
            Guid attachmentId
        )
        {

            // **2) fetch from Db and validate**
            var attach = await _context.ActivityAttachments.FindAsync(attachmentId);  // **NEW**
            if (attach == null || attach.ActivityId != activityId)
                return NotFound();

            // **3) stream the bytes back**
            return File(attach.Data, attach.ContentType, attach.FileName);            // **NEW**
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid moduleId, [FromForm] ActivityDto dto) // Bind form data + files
        {
            if (!Enum.IsDefined(typeof(ActivityType), dto.ActivityType))
                return BadRequest("Invalid activity type.");

            if (dto.ActivityType == ActivityType.Assignment && dto.EndDate == null)   
            {
                throw new Exception("Assignments must have a due date.");
            }

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

            try
            {
                dto.ModuleId = moduleId;
                var created = await _activityService.CreateActivityAsync(dto);      // Ensure association and handles dto.Files too
                return CreatedAtRoute(
                //return CreatedAtAction(
                    "GetActivityById",
                    //nameof(Get),
                    new { moduleId, activityId = created.Id },
                    created
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("{activityId}")]
        //public async Task<IActionResult> Update(Guid moduleId, Guid activityId, [FromForm] ActivityDto dto)
        //{
        //    if (activityId != dto.Id) return BadRequest("ID mismatch");

        //    if (!Enum.IsDefined(typeof(ActivityType), dto.ActivityType))
        //        return BadRequest("Invalid activity type.");
        //    // Validate the StartDate
        //    if (dto.StartDate < DateTime.UtcNow)
        //    {
        //        return BadRequest("Start date cannot be in the past.");
        //    }

        //    // Validate that EndDate (if provided) is after StartDate
        //    if (dto.EndDate.HasValue && dto.EndDate.Value <= dto.StartDate)
        //    {
        //        return BadRequest("End date must be after start date.");
        //    }

        //    dto.ModuleId = moduleId; // Ensure course association remains
        //    var updated = await _activityService.UpdateActivityAsync(dto, urls);
        //    return Ok(updated);
        //}

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(Guid moduleId, Guid activityId) =>
            Ok(await _activityService.DeleteActivityAsync(activityId));
    }
}