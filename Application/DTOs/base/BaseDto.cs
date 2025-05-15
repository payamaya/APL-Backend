namespace Application.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}