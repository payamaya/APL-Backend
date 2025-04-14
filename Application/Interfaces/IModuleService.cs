using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto?> GetModuleByIdAsync(Guid id);
        Task<ModuleDto> CreateModuleAsync(ModuleDto dto);
        Task<ModuleDto> UpdateModuleAsync(ModuleDto dto);
        Task<bool> DeleteModuleAsync(Guid id);
    }
}
