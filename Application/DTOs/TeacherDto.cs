using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Base
{
    public class TeacherDto: BasePersonDto
    {
        public TeacherType TeacherType { get; set; }
    }

}
