using Domain.Enums;

namespace Domain.Entities.Base
{
    public class Activity: BaseTimeEntity
    {
        public ActivityType ActivityType { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }

    }
}