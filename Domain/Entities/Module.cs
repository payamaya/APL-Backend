
namespace Domain.Entities
{
    public class Module
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid CourseId { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}