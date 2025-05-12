
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class EmailVerification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Token { get; set; } = null!;      // GUID or secure random string
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

    }
}
