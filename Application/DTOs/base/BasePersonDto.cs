using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Base
{
    public class BasePersonDto: BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}