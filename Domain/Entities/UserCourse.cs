using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

