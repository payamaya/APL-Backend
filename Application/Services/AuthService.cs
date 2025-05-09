using Application.DTOs;
using Application.DTOs.Auth;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepository, IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
        _userRepository = userRepository;
    }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.FindByEmailAsync(dto.Email);

            if (user == null || !PasswordHasher.Verify(dto.Password, user.Password))
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
