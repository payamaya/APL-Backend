using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Application.DTOs
{
    public class CreateUserDto
    {

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }
        public Guid? CourseId { get; set; }

        public string Description { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; }
    }
}
