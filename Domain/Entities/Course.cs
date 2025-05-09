

namespace Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        //public ICollection<Student> Students { get; set; } = new List<Student>();

        public DateTime? EndDate { get; set; } // Nullable to allow for no due date

        public DateTime? StartDate { get; set; } = DateTime.UtcNow;



    }
}
