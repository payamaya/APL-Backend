using Application.DTOs.Auth;
using Application.Helpers;
using Application.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);

            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = JwtHelper.GenerateToken(user, _config);

        return new AuthResponseDto
        {
            Token = token,
            Role = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:ExpiresInMinutes"]))
        };
    }
}

}
