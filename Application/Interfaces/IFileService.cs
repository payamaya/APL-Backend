
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> SaveFileAsync(IFormFile file, Guid activityId);
        Task<FileStreamResult> GetFileAsync(Guid fileId);
    }

}
