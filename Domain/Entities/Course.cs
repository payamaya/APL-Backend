using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
  
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public ICollection<Module> Modules { get; set; } = new List<Module>();

        public DateTime? EndDate { get; set; } // Nullable to allow for no due date

        public DateTime? StartDate { get; set; } = DateTime.UtcNow;

        //Navigation
        /*public Guid UserId { get; set; }*/
        public ICollection<User> Users { get; set; } = new List<User>();



    }
}
