
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class UserCourse
    {
        public Guid UserId { get; set; }                 // ← added
        public User User { get; set; } = null!;         // ← added

        public Guid CourseId { get; set; }               // ← added
        public Course Course { get; set; } = null!;     // ← added
    }
}

