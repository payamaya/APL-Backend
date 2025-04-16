using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public string Description { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public ActivityType ActivityType { get; set; } // ⛔ Remove `internal set`

        public Guid CourseId { get; set; }// Required foreign key
        public Guid ModuleId { get; set; } // Required foreign key
    }

}
