using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs
{
    public class UserDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool EmailConfirmed { get; set; } = false;

        public bool IsOtpVerified { get; set; } = false;
        public Guid Id { get; set; }
    }

}

