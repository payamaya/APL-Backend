using Domain.Enums;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Teacher: BasePersonEntity
    {
        public User User { get; set; } = null!;
        public TeacherType TeacherType { get; set; }       
    }
}
