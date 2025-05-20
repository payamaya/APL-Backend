using Application.DTOs.Base;
using Domain.Enums;

namespace Application.DTOs
{
    public class UserDto: BasePersonDto
    {
        public Role Role { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public bool IsOtpVerified { get; set; } = false;
    }

}

