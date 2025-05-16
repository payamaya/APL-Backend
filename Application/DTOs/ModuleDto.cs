
namespace Application.DTOs.Base
{
    public class ModuleDto: BaseTimeDto
    {
        public Guid CourseId { get; set; } // Required foreign key

    }

}
