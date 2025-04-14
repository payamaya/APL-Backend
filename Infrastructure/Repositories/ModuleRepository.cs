using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Module = Domain.Entities.Module;

namespace Infrastructure.Repositories
{
    public class ModuleService : IModuleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ModuleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto dto)
        {
            var module = _mapper.Map<Module>(dto);
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<bool> DeleteModuleAsync(Guid id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return false;
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            var modules = await _context.Modules.ToListAsync();
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(Guid id)
        {
            var module = await _context.Modules.FindAsync(id);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(ModuleDto dto)
        {
            var module = await _context.Modules.FindAsync(dto.Id);
            if (module == null) throw new Exception("Module not found");
            _mapper.Map(dto, module);
            await _context.SaveChangesAsync();
            return _mapper.Map<ModuleDto>(module);
        }
    }
}
