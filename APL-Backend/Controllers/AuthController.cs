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
            try
            {
                var result = await _authService.LoginAsync(dto);

                if (result == null)
                {
                    return Unauthorized(new { message = "Incorrect email or password." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var userId = await _authService.RegisterWithEmailConfirmationAsync(dto);
            return Ok(new { Message = "Registration successful; please check your email to confirm.", UserId = userId });
        }
        [Authorize]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.SetPasswordAsync(dto.Password);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { Success = true, result.Message });

        }


        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var success = await _authService.ConfirmEmailAsync(token);
            return success
                ? Ok(new { Message = "Email confirmed. You can now log in." })
                : throw new AppException("Email confirmation failed.");
        }

        [Authorize]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            await _authService.SendOtpAsync(email!);
            return Ok("OTP sent to your email.");
        }

        [Authorize]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody]OtpVerifyDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var ok = await _authService.VerifyOtpAsync(email!, dto.Code);
            return Ok(new
            {
                success = true,
                message = "OTP verified successfully",
            });

        }

    }

}
