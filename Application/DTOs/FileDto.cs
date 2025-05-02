using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class FileDto
    {
        [Required]
        public IFormFile File { get; set; }
        public Guid Id { get; internal set; }
        public string FileName { get; internal set; }
        //public string MimeType { get; internal set; }
        //public long FileSize { get; internal set; }
        //public DateTime UploadedAt { get; internal set; }
        //public string DownloadUrl { get; internal set; }
        public Guid ActivityId { get; set; } // example linkage
    }
}
