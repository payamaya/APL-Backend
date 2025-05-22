using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
   public class SetPasswordDto
    {

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}