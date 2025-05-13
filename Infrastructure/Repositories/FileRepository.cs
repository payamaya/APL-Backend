using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FileRepository : IBaseRepository<FileRecord>, IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileRecord?> GetByIdAsync(Guid id) =>
            await _context.FileRecords.FindAsync(id);

        public async Task AddAsync(FileRecord file) =>
            await _context.FileRecords.AddAsync(file);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public Task<IEnumerable<FileRecord>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FileRecord entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(FileRecord entity)
        {
            throw new NotImplementedException();
        }
    }
}
