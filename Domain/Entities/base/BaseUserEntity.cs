using System.ComponentModel.DataAnnotations;

namespace   Domain.Entities.Base
{
    public class BaseUserEntity
    {
        [Key]
        public Guid UserId { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}