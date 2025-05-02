using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFileRepository
    {
        Task<FileRecord?> GetByIdAsync(Guid id);
        Task AddAsync(FileRecord file);
        Task SaveChangesAsync();
    }
}
