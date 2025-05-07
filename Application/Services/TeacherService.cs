using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("create-teacher")]
        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto)
        {
            var teacher = _mapper.Map<Teacher>(dto);
            await _repository.AddAsync(teacher);
            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var teacher = await _repository.GetByIdAsync(id);
            if (teacher == null) return false;

            await _repository.DeleteAsync(teacher);
            return true;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await _repository.GetByIdAsync(id);
            return teacher == null ? null : _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto> UpdateTeacherAsync(TeacherDto dto)
        {
            var teacher = await _repository.GetByIdAsync(dto.Id);
            if (teacher == null) throw new Exception("Teacher not found");

            _mapper.Map(dto, teacher);
            await _repository.UpdateAsync(teacher);
            return _mapper.Map<TeacherDto>(teacher);
        }
    }
}
