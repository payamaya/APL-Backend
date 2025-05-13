
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeacherRepository : IBaseRepository<Teacher>, ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(Guid id)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Teacher teacher)
        {
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
