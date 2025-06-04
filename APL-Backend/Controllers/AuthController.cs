using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Base;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APL_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto); // throws exceptions if invalid
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var userId = await _authService.RegisterWithEmailConfirmationAsync(dto); // throws on failure
            return Ok(new
            {
                Message = "Registration successful; please check your email to confirm.",
                UserId = userId
            });
        }

        [Authorize]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                throw new AppException("Invalid password input.");

            var result = await _authService.SetPasswordAsync(dto.Password);
            if (!result.Success)
                throw new AppException(result.Message);

            return Ok(new { success = true, result.Message });
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var success = await _authService.ConfirmEmailAsync(token);
            if (!success)
                throw new AppException("Email confirmation failed.");

            return Ok(new { Message = "Email confirmed. You can now log in." });
        }

        [Authorize]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedException("Email not found in claims.");

            await _authService.SendOtpAsync(email);
            return Ok(new { Message = "OTP sent to your email." });
        }

        [Authorize]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedException("Email not found in claims.");

            await _authService.VerifyOtpAsync(email, dto.Code);
            return Ok(new
            {
                success = true,
                message = "OTP verified successfully"
            });
        }
    }
}
