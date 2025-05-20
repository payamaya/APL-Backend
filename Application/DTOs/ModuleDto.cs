
using Application.DTOs.Base;

namespace Application.DTOs
{
    public class ModuleDto: BaseTimeDto
    {
        public Guid CourseId { get; set; } // Required foreign key

    }

}
