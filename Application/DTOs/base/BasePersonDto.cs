using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Base
{
    public class BasePersonDto: BaseUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty; // ⛔ Remove `internal set`

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;

    }
}