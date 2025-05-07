using Domain.Enums;

namespace Application.DTOs
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }

}

