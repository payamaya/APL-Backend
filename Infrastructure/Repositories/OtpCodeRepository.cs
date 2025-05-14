using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OtpCodeRepository : BaseRepository<OtpCode>, IOtpCodeRepository
    {
        public OtpCodeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<OtpCode?> GetLatestValidOtpByEmailAsync(string email)
        {
            return await _context.OtpCodes
                .Where(o => o.Email == email && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
