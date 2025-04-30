
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public TeacherType TeacherType { get; set; }
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; }

        //public User User { get; set; }
        //public Guid UserId { get; set; }

    }
}
