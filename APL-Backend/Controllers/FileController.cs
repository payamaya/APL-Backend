using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
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
                var fileRecord = await _fileService.GetFileRecordAsync(id); // you may need this helper
                if (fileRecord == null)
                    return NotFound();
            try
            {
                var decryptedData = await _fileService.DownloadFileAsync(id); // returns decrypted bytes
                if (decryptedData == null || decryptedData.Length == 0)
                    return NotFound("Decrypted file content not found or is empty.");

                return File(decryptedData, fileRecord.MimeType, fileRecord.FileName);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 400 if decryption fails
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while downloading the file.");
            }
        }
    }
}
