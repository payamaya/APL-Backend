using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Base
{
    public class BasePersonDto: BaseUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;

    }
}