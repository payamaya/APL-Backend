using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Authorize(Policy = "RequireTeacher")]
    [Route("api/course/{courseId}/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid courseId) =>
            Ok(await _moduleService.GetAllModulesAsync(courseId));

        [HttpGet("{moduleId}")]
        public async Task<IActionResult> Get(Guid courseId, Guid moduleId)
        {
            var result = await _moduleService.GetModuleByIdAsync(courseId, moduleId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid courseId, [FromBody] ModuleDto dto)
        {
            try
            {
                dto.CourseId = courseId; // Ensure association
                var createdModule = await _moduleService.CreateModuleAsync(dto);
                return CreatedAtAction(
                    nameof(Get),
                    new { courseId, moduleId = createdModule.Id },
                    createdModule
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{moduleId}")]
        public async Task<IActionResult> Update(Guid courseId, Guid moduleId, [FromBody] ModuleDto dto)
        {
            if (moduleId != dto.Id) return BadRequest("ID mismatch");
            dto.CourseId = courseId; // Ensure course association remains
            return Ok(await _moduleService.UpdateModuleAsync(dto));
        }

        [HttpDelete("{moduleId}")]
        public async Task<IActionResult> Delete(Guid courseId, Guid moduleId) =>
            Ok(await _moduleService.DeleteModuleAsync(moduleId));
    }

 
}



