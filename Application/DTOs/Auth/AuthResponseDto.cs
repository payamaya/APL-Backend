
namespace Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

}
