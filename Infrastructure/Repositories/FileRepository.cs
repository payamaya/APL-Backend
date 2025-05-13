using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class FileRepository : BaseRepository<FileRecord>, IFileRepository
    {
        public FileRepository(AppDbContext context) : base(context)
        {
        }

        // You can add custom methods later if needed
    }
}
