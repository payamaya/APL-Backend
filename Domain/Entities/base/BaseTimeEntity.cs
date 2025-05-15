namespace Domain.Entities.Base
{
    public class BaseTimeEntity: BaseEntity
    {
        public DateTime? EndDate { get; set; } // Nullable to allow for no due date
        public DateTime? StartDate { get; set; } = DateTime.UtcNow;
    }
}