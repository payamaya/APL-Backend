using Domain.Enums;


namespace Domain.Entities.Base
{
    public class User: BaseUserEntity
    {
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public Role Role { get; set; } // "Admin", "Teacher", "Student"
        public bool IsOtpVerified { get; set; }
        // ← added: navigation for enrollments
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }

}
