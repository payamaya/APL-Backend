using System.ComponentModel.DataAnnotations;
using Domain.Enums;


namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty; // Optional
        //TODO change Role to string
        public Role Role { get; set; }

        //Navigation
        public Guid CourseId { get; set; }
     /*   public Course Course { get; set; }*/
        public ICollection<File> Courses { get; set; } = new List<File>();




    }
}
