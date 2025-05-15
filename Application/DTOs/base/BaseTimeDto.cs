namespace Application.DTOs.Base
{
    public class BaseTimeDto: BaseDto
    {
        public DateTime StartDate { get; set; } // Ensure this is a DateTime
        public DateTime? EndDate { get; set; }

    }
}