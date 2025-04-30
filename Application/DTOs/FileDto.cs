using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FileDto
    {
        public IFormFile File { get; set; }
        public int ActivityId { get; set; } // example linkage
    }
}
