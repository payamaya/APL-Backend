using Application.DTOs;


namespace Application.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync(Guid courseId);
        Task<ModuleDto?> GetModuleByIdAsync(Guid courseId, Guid moduleId);
        Task<ModuleDto> CreateModuleAsync(ModuleDto dto);
        Task<ModuleDto> UpdateModuleAsync(ModuleDto dto);
        Task<bool> DeleteModuleAsync(Guid moduleId);
    }
}