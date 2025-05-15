using Application.DTOs;
using Application.DTOs.Base;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Base;
using Domain.Enums;
using Domain.Interfaces;
   
using Infrastructure.Data;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repos;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(IRepositoryWrapper repos, AppDbContext context, IMapper mapper)
    {
        _repos = repos;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsync(UserDto dto)
    {
        // Update the method call to specify the correct interface or namespace to resolve ambiguity
        var existing = await _repos.Users.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new ConflictException("Email already in use.");

        var user = _mapper.Map<User>(dto);
        user.UserId = Guid.NewGuid();
        user.Password = PasswordHasher.Hash(dto.Password); // Assuming dto.Password is provided
        user.CreatedAt = DateTime.UtcNow;

        await _repos.Users.AddAsync(user);
        await _repos.Users.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }


    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _repos.Users.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _repos.Users.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserAsync(UserDto dto)
    {
        var user = await _repos.Users.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new Exception("User not found.");

        // Update fields (but don't update sensitive fields unless explicitly required)
        user.Email = dto.Email;
        user.Role = dto.Role;
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.Password = PasswordHasher.Hash(dto.Password);
        }

        await _repos.Users.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _repos.Users.GetByIdAsync(id);
        if (user == null)
            return false;

        _repos.Users.DeleteAsync(user);
        await _repos.Users.SaveChangesAsync();
        return true;
    }

    // ← added: enroll a user in a course
    public async Task AssignUserToCourseAsync(AssignUserToCourseDto dto)
    {
        var exists = await _context.UserCourses.FindAsync(dto.UserId, dto.CourseId);
        if (exists != null)
            throw new InvalidOperationException("User already enrolled in this course.");

        var uc = new UserCourse
        {
            UserId = dto.UserId,
            CourseId = dto.CourseId
        };
        _context.UserCourses.Add(uc);
        await _context.SaveChangesAsync();
    }

    // ← added: remove a user from a course
    public async Task RemoveUserFromCourseAsync(AssignUserToCourseDto dto)
    {
        var uc = await _context.UserCourses.FindAsync(dto.UserId, dto.CourseId);
        if (uc == null)
            throw new InvalidOperationException("User is not enrolled in this course.");

        _context.UserCourses.Remove(uc);
        await _context.SaveChangesAsync();
    }
}
