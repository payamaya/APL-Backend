
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Student
    {
        [Key]
        public Guid UserId { get; set; }  // Acts as both PK and FK
        public User User { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;        
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
