namespace Application.DTOs.Base
{
    public class BaseTimeDto: BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } // Ensure this is a DateTime
        public DateTime? EndDate { get; set; }

    }
}