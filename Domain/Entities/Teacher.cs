using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class Teacher
    {
        [Key]
        public Guid UserId { get; set; }  // Acts as both PK and FK

        public User User { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

        public string? Title { get; set; } = string.Empty;
        public TeacherType TeacherType { get; set; }
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
