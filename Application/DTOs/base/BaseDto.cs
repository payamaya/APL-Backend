namespace Application.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}