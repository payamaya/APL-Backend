using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(20)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        //public ICollection<CourseDto> Courses { get; set; } = new List<CourseDto>();
        
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
