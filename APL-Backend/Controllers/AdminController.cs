//using Application.DTOs;
//using Application.Interfaces;
//using Infrastructure.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace APL_Backend.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminController : ControllerBase
//    {
//        private readonly IUserService _userService;

//        public AdminController(IUserService userService)
//        {
//            _userService = userService;
//        }

//        [HttpPost("create-user")]
//        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
//        {
//            var user = await _userService.CreateUserAsync(dto);
//            return Ok(user);
//        }

//        [HttpPost("assign-course")]
//        public async Task<IActionResult> AssignToCourse([FromBody] AssignUserToCourseDto dto)
//        {
//            await _userService.AssignUserToCourseAsync(dto);
//            return Ok("Assigned Successfully!");
//        }

//        //[HttpGet("teachers")]
//        //public async Task<IActionResult> GetAllTeachers()
//        //{
//        //    var users = await _userService.GetAllTeachersAsync();
//        //    return Ok(users);
//        //}

//    }
//}
