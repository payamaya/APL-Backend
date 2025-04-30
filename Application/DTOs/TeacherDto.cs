using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public TeacherType TeacherType { get; set; }

        //[EmailAddress]
        //[MaxLength(20)]
        public string Email { get; set; } = string.Empty; // ⛔ Remove `internal set`

        //[Phone]
        //[MaxLength(20)]
        public string Telephone { get; set; }


    }

}
