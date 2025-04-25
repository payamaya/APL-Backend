using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ?Password { get; set; } = string.Empty; // Optional
        public Enums.Role Role { get; set; }

        //Navigation
        public ICollection<Course> Courses { get; set; } = new List<Course>();



    }
}
