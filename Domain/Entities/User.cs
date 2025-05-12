using System.ComponentModel.DataAnnotations;
using Domain.Enums;


namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public Role Role { get; set; } // "Admin", "Teacher", "Student"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOtpVerified { get; set; }
    }

}
