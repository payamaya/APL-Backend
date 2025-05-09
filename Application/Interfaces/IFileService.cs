
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<Guid> SaveFileAsync(FileDto dto);
        Task<FileRecord?> GetFileRecordAsync(Guid id);
        Task<byte[]> DownloadFileAsync(Guid id);

    }

}
