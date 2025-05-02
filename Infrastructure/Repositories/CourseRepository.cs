/*
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CourseService: ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateCourseAsync(CourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            return course == null ? null : _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(CourseDto dto)
        {
            var course = await _context.Courses.FindAsync(dto.Id);
            if (course == null) throw new Exception("Course not found");
            _mapper.Map(dto, course);
            await _context.SaveChangesAsync();
            return _mapper.Map<CourseDto>(course);
        }
      
    }
}
*/
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.File>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Domain.Entities.File?> GetByIdAsync(Guid id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddAsync(Domain.Entities.File course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain.Entities.File course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Domain.Entities.File course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
