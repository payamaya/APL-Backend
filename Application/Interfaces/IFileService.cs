
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<Guid> SaveFileAsync(FileDto dto);
        Task<FileRecord?> GetFileRecordAsync(Guid id);
        Task<byte[]> DownloadFileAsync(Guid id);

    }

}
