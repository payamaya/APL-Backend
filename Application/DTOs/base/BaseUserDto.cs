using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Base
{
    public class BaseUserDto: BaseDto
    {
        //public Guid UserId { get; set; }
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}