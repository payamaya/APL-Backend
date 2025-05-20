using Application.DTOs.Base;
using Domain.Enums;

namespace Application.DTOs
{
    public class TeacherDto: BasePersonDto
    {
        public TeacherType TeacherType { get; set; }
    }

}
