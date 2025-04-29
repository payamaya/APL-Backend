/*using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Enums;

namespace Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AssignUserToCourseAsync(AssignUserToCourseDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
                throw new Exception("User not found");

            if (user.Role != Role.Teacher && user.Role != Role.Student)
                throw new Exception("Only Teachers or Students can be assigned to courses");

            var course = await _context.Courses.FindAsync(dto.CourseId);
            if (course == null)
                throw new Exception("Course not found");

            // Prevent duplicates
            if (!user.Courses.Any(c => c.Id == dto.CourseId))
            {
                user.Courses.Add(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}
*/