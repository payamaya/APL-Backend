using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ModuleRepository : BaseRepository<Module>, IModuleRepository
{
    public ModuleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Module>> GetAllAsync(Guid courseId)
    {
        return await _context.Modules
            .Where(m => m.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Module?> GetByIdAsync(Guid courseId, Guid moduleId)
    {
        return await _context.Modules
            .FirstOrDefaultAsync(m => m.Id == moduleId && m.CourseId == courseId);
    }
}
