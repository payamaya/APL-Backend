namespace Application.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
/*        public ICollection<CourseDto> Courses { get; set; } = new List<CourseDto>();*/
    
        // Ensure this is a DateTime
        /*
          public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } // Nullable to allow for no due date
        */
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
