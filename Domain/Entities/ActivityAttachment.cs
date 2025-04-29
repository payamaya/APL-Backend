namespace Domain.Entities
{
    public class ActivityAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK → Activity
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        // File metadata
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        // Actual file bytes → maps automatically to Postgres bytea
        public byte[] Data { get; set; } = Array.Empty<byte>();

    }
}
