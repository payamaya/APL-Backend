using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Enums;
using TimeZoneConverter;

//namespace Domain.Entities
//{
//    public class Activity
//    {
//        public Guid Id { get; set; } = Guid.NewGuid();
//        public string Name { get; set; } = string.Empty;
//        public string Title { get; set; } = string.Empty;
//        public string Description { get; set; } = string.Empty;
//        public string Content { get; set; } = string.Empty;
//        public ActivityType ActivityType { get; set; }
//        public Guid ModuleId { get; set; }
//        public Module Module { get; set; }
//        public DateTime? EndDate { get; set; } = DateTime.UtcNow; // Nullable if you want to allow for no due date
//        public DateTime? StartDate { get; set; } = DateTime.UtcNow;
//        public List<string> AttachmentUrls { get; set; } = new(); // New property to hold the list of saved file URLs
//        public List<ActivityAttachment> Attachments { get; set; } = new(); // Navigation property:

//    }
//}

namespace Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ActivityType ActivityType { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        // ← JUST URLs, stored as JSON
        public List<string> AttachmentUrls { get; set; } = new();

        // **NEW** navigation for attachments table
        public List<ActivityAttachment> Attachments { get; set; } = new();
    }
}
