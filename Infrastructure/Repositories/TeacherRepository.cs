/*using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Teacher = Domain.Entities.Teacher;


namespace Infrastructure.Repositories
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TeacherService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto)
        {
            var Teacher = _mapper.Map<Teacher>(dto);
            _context.Teachers.Add(Teacher);
            await _context.SaveChangesAsync();
            return _mapper.Map<TeacherDto>(Teacher);
        }

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var Teacher = await _context.Teachers.FindAsync(id);
            if (Teacher == null) return false;
            _context.Teachers.Remove(Teacher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var Teachers = await _context.Teachers.ToListAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(Teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var Teacher = await _context.Teachers.FindAsync(id);
            return Teacher == null ? null : _mapper.Map<TeacherDto>(Teacher);
        }

        public async Task<TeacherDto> UpdateTeacherAsync(TeacherDto dto)
        {
            var Teacher = await _context.Teachers.FindAsync(dto.Id);
            if (Teacher == null) throw new Exception("Teacher not found");
            _mapper.Map(dto, Teacher);
            await _context.SaveChangesAsync();
            return _mapper.Map<TeacherDto>(Teacher);
        }
    }
}
*/
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(Guid id)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
