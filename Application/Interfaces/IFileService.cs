
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<Guid> SaveFileAsync(IFormFile file, Guid activityId);
        Task<byte[]> DownloadFileAsync(Guid id);

    }

}
