namespace Domain.Entities.Base
{
    public class BaseVerificationEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}