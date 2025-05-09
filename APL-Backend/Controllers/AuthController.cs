using Application.DTOs;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            var userId = await _userService.RegisterAsync(dto);
            return Ok(new { Message = "User registered successfully", UserId = userId });
        }
    }

}
