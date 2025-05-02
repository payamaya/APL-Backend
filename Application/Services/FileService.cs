using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly string _uploadPath;

        public FileService(IFileRepository fileRepository, string uploadPath)
        {
            _fileRepository = fileRepository;
            _uploadPath = uploadPath;

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<Guid> SaveFileAsync(FileDto dto)
        {
            dto.Id = Guid.NewGuid();
            var fileName = $"{Path.GetFileName(dto.FileName)}";
            var fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var entity = new FileRecord
            {
                Id = dto.Id,
                FileName = fileName,
                FilePath = fullPath,
                Size = dto.File.Length,
                MimeType = string.IsNullOrWhiteSpace(dto.File.ContentType)
                    ? "application/octet-stream"
                    : dto.File.ContentType,
                ActivityId = dto.ActivityId,
                UploadedAt = DateTime.UtcNow
            };

            await _fileRepository.AddAsync(entity);
            await _fileRepository.SaveChangesAsync();

            return dto.Id;
        }

        public async Task<byte[]> DownloadFileAsync(Guid id)
        {
            var fileRecord = await _fileRepository.GetByIdAsync(id);
            if (fileRecord == null || !System.IO.File.Exists(fileRecord.FilePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            return await System.IO.File.ReadAllBytesAsync(fileRecord.FilePath);
        }
    }
}
