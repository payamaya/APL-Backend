using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Base
{
    public class UserDto: BasePersonDto
    {
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public bool IsOtpVerified { get; set; } = false;
    }

}

