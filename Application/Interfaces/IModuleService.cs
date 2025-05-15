using Application.DTOs.Base;
using Application.Interfaces.Base;

namespace Application.Interfaces
{
    public interface IModuleService: ICrudService<ModuleDto>
    {
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync(Guid courseId);
        Task<ModuleDto?> GetModuleByIdAsync(Guid courseId, Guid moduleId);
        // Task<ModuleDto> CreateModuleAsync(ModuleDto dto);
        // Task<ModuleDto> UpdateModuleAsync(ModuleDto dto);
        // Task<bool> DeleteModuleAsync(Guid moduleId);
    }
}