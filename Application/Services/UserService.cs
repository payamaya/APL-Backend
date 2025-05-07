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
