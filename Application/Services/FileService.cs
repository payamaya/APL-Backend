using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly AppDbContext _context;
        private readonly string _uploadPath;

        public FileService(AppDbContext context, string uploadPath)
        {
            _context = context;
            _uploadPath = uploadPath;

            // Ensure the upload directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<Guid> SaveFileAsync(IFormFile file, Guid activityId)
        {
            var fileId = Guid.NewGuid();
            var fileName = $"{fileId}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var entity = new FileRecord
            {
                Id = fileId,
                FileName = file.FileName,
                FilePath = fullPath,
                Size = file.Length,
                MimeType = string.IsNullOrWhiteSpace(file.ContentType)
                    ? "application/octet-stream"
                    : file.ContentType,
                ActivityId = activityId,
                UploadedAt = DateTime.UtcNow
            };

            _context.FileRecords.Add(entity);
            await _context.SaveChangesAsync();

            return fileId;
        }

        public async Task<byte[]> DownloadFileAsync(Guid id)
        {
            var fileRecord = await _context.FileRecords.FirstOrDefaultAsync(f => f.Id == id);
            if (fileRecord == null || !File.Exists(fileRecord.FilePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            return await File.ReadAllBytesAsync(fileRecord.FilePath);
        }
    }
}
