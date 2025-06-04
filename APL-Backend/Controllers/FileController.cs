using Application.DTOs;
using Application.Exceptions;
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
                throw new AppException("No file provided.");

            var fileId = await _fileService.SaveFileAsync(dto);
            return Ok(new { FileId = fileId });
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var fileRecord = await _fileService.GetFileRecordAsync(id);
            if (fileRecord == null)
                throw new NotFoundException($"File with ID {id} was not found.");

            var decryptedData = await _fileService.DownloadFileAsync(id);
            if (decryptedData == null || decryptedData.Length == 0)
                throw new NotFoundException("Decrypted file content not found or is empty.");

            return File(decryptedData, fileRecord.MimeType, fileRecord.FileName);
        }

    }
}
