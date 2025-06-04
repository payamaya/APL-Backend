using System.Security.Cryptography;
using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly string _uploadPath;
        private readonly EncryptionHelper _encryptionHelper; // Add an instance of EncryptionHelper  

        public FileService(IFileRepository fileRepository, string uploadPath, EncryptionHelper encryptionHelper)
        {
            _fileRepository = fileRepository;
            _uploadPath = uploadPath;
            _encryptionHelper = encryptionHelper; // Initialize the EncryptionHelper instance  

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<FileRecord?> GetFileRecordAsync(Guid id)
        {
            var fileRecord = await _fileRepository.GetByIdAsync(id);
            if (fileRecord == null || !System.IO.File.Exists(fileRecord.FilePath))
            {
                throw new NotFoundException("File not found.");
            }
            return fileRecord;
        }
        public async Task<Guid> SaveFileAsync(FileDto dto)
        {
            // Limiting file types and size  
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(dto.File.FileName).ToLower();
            if (!allowedExtensions.Contains(extension) || dto.File.Length > 10 * 1024 * 1024) // 10MB max  
            {
                throw new AppException("Invalid file type or file size exceeds the limit.");
            }

            // Generate a unique ID for the file  
            dto.Id = Guid.NewGuid();
            var originalExtension = Path.GetExtension(dto.File.FileName); // e.g., ".pdf", ".jpg"  
            var baseName = Path.GetFileNameWithoutExtension(dto.FileName); // Just the name, no extension  
            var fileName = $"{baseName}{originalExtension}"; // Reconstruct with extension  
            var fullPath = Path.Combine(_uploadPath, fileName);

            using (var ms = new MemoryStream())
            {
                await dto.File.CopyToAsync(ms);
                ms.Position = 0; // Reset the stream position to the beginning
                var encryptedData = _encryptionHelper.Encrypt(ms.ToArray()); // Use the instance of EncryptionHelper  
                await File.WriteAllBytesAsync(fullPath, encryptedData);
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
                throw new NotFoundException("File not found.");
            }

            // Step 1: Read the encrypted file
            var encryptedData = await File.ReadAllBytesAsync(fileRecord.FilePath);
            try
            {
                // Step 2: Decrypt the content
                var decryptedData = _encryptionHelper.Decrypt(encryptedData);
                return decryptedData;
            }
            catch (CryptographicException) 
            {
                throw new AppException("Failed to decrypt the file. It may be corrupted or the encryption key is invalid.");
            }

            // Step 3: Return decrypted content
        }
    }
}
