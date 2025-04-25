using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


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

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            // Optionally hash password
            // user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task AssignUserToCourseAsync(AssignUserToCourseDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var course = await _context.Courses.FindAsync(dto.CourseId);

            if (user == null || course == null)
                throw new Exception("User or Course not found");

            user.Courses.Add(course);
            await _context.SaveChangesAsync();
        }
    }
}
