using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OtpCodeRepository : IBaseRepository<OtpCode>, IOtpCodeRepository
    {
        private readonly AppDbContext _context;

        public OtpCodeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OtpCode?> GetLatestValidOtpByEmailAsync(string email)
        {
            return await _context.OtpCodes
               .Where(o => o.Email == email && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
               .OrderByDescending(o => o.CreatedAt)
               .FirstOrDefaultAsync();
        }

        public async Task AddAsync(OtpCode otpCode)
        {
            await _context.OtpCodes.AddAsync(otpCode);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<OtpCode>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OtpCode?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OtpCode entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(OtpCode entity)
        {
            throw new NotImplementedException();
        }
    }

}
