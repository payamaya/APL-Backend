using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Enums;
using TimeZoneConverter;

namespace Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ActivityType ActivityType { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }

        public DateTime? DueDate { get; set; } // Nullable to allow for no due date

        public DateTime? CreatedAt { get; set; } =
        TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
    );

    }
}