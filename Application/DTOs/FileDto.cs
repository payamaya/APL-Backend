using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class FileDto
    {
        public IFormFile? File { get; set; }
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public Guid ActivityId { get; set; } // Example FK
    }
}
