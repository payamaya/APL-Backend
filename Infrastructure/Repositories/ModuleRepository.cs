
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ModuleRepository : IBaseRepository<Module>, IModuleRepository
    {
        private readonly AppDbContext _context;

        public ModuleRepository(AppDbContext context)
        {
            _context = context;
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

        public async Task AddAsync(Module module)
        {
            await _context.Modules.AddAsync(module);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Module module)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Module module)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Module>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Module?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
