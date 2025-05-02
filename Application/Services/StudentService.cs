using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
        {
            var student = _mapper.Map<Student>(dto);
            student.CreatedAt = DateTime.UtcNow; // Ensure createdAt is set

            await _repository.AddAsync(student);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null) return false;

            await _repository.DeleteAsync(student);
            return true;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            return student == null ? null : _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> UpdateStudentAsync(StudentDto dto)
        {
            var student = await _repository.GetByIdAsync(dto.Id);
            if (student == null) throw new Exception("Student not found");

            _mapper.Map(dto, student);
            await _repository.UpdateAsync(student);
            return _mapper.Map<StudentDto>(student);
        }
    }
}
