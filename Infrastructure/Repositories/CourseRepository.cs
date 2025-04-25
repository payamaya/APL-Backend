using Application.DTOs;
using Application.Interfaces;
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
