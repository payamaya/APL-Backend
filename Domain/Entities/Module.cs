using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Module: BaseTimeEntity
    {
        public Guid CourseId { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}