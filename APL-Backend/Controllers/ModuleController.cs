using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/course/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetAll() => Ok(await _moduleService.GetAllModulesAsync());

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _moduleService.GetModuleByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([FromBody] ModuleDto dto) => Ok(await _moduleService.CreateModuleAsync(dto));

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ModuleDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            return Ok(await _moduleService.UpdateModuleAsync(dto));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _moduleService.DeleteModuleAsync(id));
    }
}
