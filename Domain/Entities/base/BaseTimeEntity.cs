namespace Domain.Entities.Base
{
    public class BaseTimeEntity: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; } // Nullable to allow for no due date
        public DateTime? StartDate { get; set; } = DateTime.UtcNow;
    }
}