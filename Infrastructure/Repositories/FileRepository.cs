using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileRecord?> GetByIdAsync(Guid id) =>
            await _context.FileRecords.FindAsync(id);

        public async Task AddAsync(FileRecord file)
        {
            await _context.FileRecords.AddAsync(file);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
