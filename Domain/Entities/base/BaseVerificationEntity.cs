namespace Domain.Entities.Base
{
    public class BaseVerificationEntity: BaseEntity
    {
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}