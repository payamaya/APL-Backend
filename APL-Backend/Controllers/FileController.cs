using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APL_Backend.Controllers
{
    public class FileController
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            // Generate unique file name
            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var savePath = Path.Combine("wwwroot/uploads", fileName);

            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Save metadata to DB
            var fileRecord = new FileRecord
            {
                FileName = dto.File.FileName,
                FilePath = savePath,
                Size = dto.File.Length,
                ActivityId = dto.ActivityId
            };

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Uploaded!", fileId = fileRecord.Id });
        }

    }
}
