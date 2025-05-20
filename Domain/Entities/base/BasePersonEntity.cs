
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base
{
    public abstract class BasePersonEntity: BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
