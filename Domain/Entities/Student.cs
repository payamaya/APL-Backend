
namespace Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;        
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

    /*    public ICollection<Course> Courses { get; set; } = new List<Course>();*/
        // Ensure this is a DateTime
        /*
          public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } // Nullable to allow for no due date
        */
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
