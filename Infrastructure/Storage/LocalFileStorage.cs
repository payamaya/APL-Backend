using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _basePath;

        public LocalFileStorage(IConfiguration config)
        {
            // now GetValue<string> works because of Configuration.Binder
            _basePath = config.GetValue<string>("FileStorage:LocalPath")
                        ?? Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(_basePath);
        }


        public async Task<string> SaveAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_basePath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Return a URL or relative path that your front-end can use:
            return $"/uploads/{fileName}";
        }
    }
}
