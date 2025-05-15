
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Course: BaseTimeEntity
    {
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    }
}
