using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class OtpVerifyDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
    }

}
