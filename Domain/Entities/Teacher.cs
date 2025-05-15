using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities.Base
{
    public class Teacher: BasePersonEntity
    {
        public User User { get; set; } = null!;
        public TeacherType TeacherType { get; set; }       
    }
}
