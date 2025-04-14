using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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