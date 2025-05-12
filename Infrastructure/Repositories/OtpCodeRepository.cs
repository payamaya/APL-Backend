using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OtpCodeRepository : IOtpCodeRepository
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
    }

}
