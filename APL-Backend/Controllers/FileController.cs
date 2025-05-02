using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file provided.");

            var fileId = await _fileService.SaveFileAsync(dto);
            return Ok(new { FileId = fileId });
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            try
            {
                var bytes = await _fileService.DownloadFileAsync(id);
                return File(bytes, "application/octet-stream", $"file_{id}");
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found.");
            }
        }
    }
}
