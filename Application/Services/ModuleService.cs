using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DTOs.Base;

namespace Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _repository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public ModuleService(IModuleRepository repository, IMapper mapper, ICourseRepository courseRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync(Guid courseId)
        {
            var modules = await _repository.GetAllAsync(courseId);
            return _mapper.Map<IEnumerable<ModuleDto>>(modules);
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(Guid courseId, Guid moduleId)
        {
            var module = await _repository.GetByIdAsync(courseId, moduleId);
            return module == null ? null : _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto dto)
        {
            // Verify the course exists first
            var courseExists = await _courseRepository.GetByIdAsync(dto.CourseId);
            if (courseExists == null)
            {
                throw new InvalidOperationException($"Course with ID {dto.CourseId} does not exist");
            }

            var module = _mapper.Map<Module>(dto);
            await _repository.AddAsync(module);
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> UpdateModuleAsync(ModuleDto dto)
        {
            var module = await _repository.GetByIdAsync(dto.CourseId, dto.Id);
            if (module == null) throw new Exception("Module not found");

            // Verify the course exists if CourseId is being updated
            if (module.CourseId != dto.CourseId)
            {
                var courseExists = await _courseRepository.GetByIdAsync(dto.CourseId);
                if (courseExists == null)
                {
                    throw new InvalidOperationException($"Course with ID {dto.CourseId} does not exist");
                }
            }

            _mapper.Map(dto, module);
            await _repository.UpdateAsync(module);
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<bool> DeleteModuleAsync(Guid moduleId)
        {
            var module = await _repository.GetByIdAsync(Guid.Empty, moduleId); // Adjust if necessary for courseId
            if (module == null) return false;

            await _repository.DeleteAsync(module);
            return true;
        }
    }
}
