using Domain.Entities.Enums;


namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ?Password { get; set; } = string.Empty; // Optional
        //TODO change Role to string
        public Role Role { get; set; }

        //Navigation
        public Guid CourseId { get; set; }
     /*   public Course Course { get; set; }*/
        public ICollection<Course> Courses { get; set; } = new List<Course>();




    }
}
