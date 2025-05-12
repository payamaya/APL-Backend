using Application.DTOs;
using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<Guid> RegisterWithEmailConfirmationAsync(UserDto dto);

        Task<bool> ConfirmEmailAsync(string token);
        Task SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string code);
    }

}
