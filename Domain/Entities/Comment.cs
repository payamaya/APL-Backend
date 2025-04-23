using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ActivityId { get; set; } // Required foreign key
        public Guid UserId { get; set; } // Required foreign key
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Optional for edits
        // Navigation properties
        public virtual Activity Activity { get; set; } = null!;

        //public virtual User User { get; set; } = null!;
    }
}
