using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> RegisterAsync(UserDto dto)
    {
        // 1. Check if email exists
        var existingUser = await _userRepository.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already in use.");

        // 2. Hash the password
        var passwordHash = PasswordHasher.Hash(dto.Password);

        // 3. Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = dto.Role
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user.Id;
    }

}
