using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid ActivityId { get; set; } // Required foreign key
        public Guid UserId { get; set; } // Required foreign key
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } // Optional for edits
    }
}
