
namespace Domain.Entities
{
    public class OtpCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Code { get; set; } = null!;       // e.g. 6 digits
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Email { get; set; } = string.Empty; // Optional, if you want to store the email
    }
}
