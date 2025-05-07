using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repos;
    private readonly AppDbContext _context;

    public UserService(IRepositoryWrapper repos, AppDbContext context)
    {
        _repos = repos;
        _context = context;
    }

    public Task AssignUserToCourseAsync(UserDto dto)
    {
        throw new NotImplementedException();
    }
    public async Task<Guid> CreateUserAsync(UserDto dto)
    {
        // Check if email already exists
        var existingUser = await _repos.Users.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new ConflictException("Email is already in use.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Hash the password
            var passwordHash = PasswordHasher.Hash(dto.Password);

            // Create core User entity
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _repos.Users.AddAsync(user);
            await _repos.Users.SaveChangesAsync();

            // Create a corresponding domain entity
            switch (dto.Role)
            {
                case Role.Student:
                    var student = new Student
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        // Other student-specific fields if needed
                    };
                    await _repos.Students.AddAsync(student);
                    break;

                case Role.Teacher:
                    var teacher = new Teacher
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        // Other teacher-specific fields if needed
                    };
                    await _repos.Teachers.AddAsync(teacher);
                    break;

                case Role.Admin:
                    throw new InvalidOperationException("Manual admin creation is not allowed.");
            }

            await transaction.CommitAsync();
            return user.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Guid> RegisterAsync(UserDto dto)
    {
        var existing = await _repos.Users.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new ConflictException("Email already in use.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _repos.Users.AddAsync(user);
            await _repos.Users.SaveChangesAsync();

            switch (dto.Role)
            {
                case Role.Student:
                    await _repos.Students.AddAsync(new Student { UserId = user.Id, Email = user.Email });
                    break;
                case Role.Teacher:
                    await _repos.Teachers.AddAsync(new Teacher { UserId = user.Id, Email = user.Email });
                    break;
                case Role.Admin:
                    throw new InvalidOperationException("Admins cannot be registered manually.");
            }

            await transaction.CommitAsync();
            return user.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
