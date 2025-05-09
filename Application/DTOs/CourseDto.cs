namespace Application.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public string Description { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public DateTime StartDate { get; set; } // Ensure this is a DateTime
        public DateTime? EndDate { get; set; }

    }

}
