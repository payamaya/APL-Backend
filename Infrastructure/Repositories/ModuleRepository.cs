using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync(Guid courseId)
        {
            var modules = await _context.Modules
                .Where(m => m.CourseId == courseId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(Guid courseId, Guid moduleId)
        {
            var module = await _context.Modules
                .FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == courseId);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto dto)
        {
            // Verify the course exists first
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!courseExists)
            {
                throw new InvalidOperationException($"Course with ID {dto.CourseId} does not exist");
            }

            var module = _mapper.Map<Module>(dto);
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(ModuleDto dto)
        {
            var module = await _context.Modules.FindAsync(dto.Id);
            if (module == null) throw new Exception("Module not found");

            // Verify the course exists if CourseId is being updated
            if (module.CourseId != dto.CourseId)
            {
                var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
                if (!courseExists)
                {
                    throw new InvalidOperationException($"Course with ID {dto.CourseId} does not exist");
                }
            }

            _mapper.Map(dto, module);
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

        // Remove this method as it's not in the interface
        // public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        // {
        //     var modules = await _context.Modules.ToListAsync();
        //     return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        // }
    }
}