using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class EmailVerificationRepository: IBaseRepository<EmailVerification>, IEmailVerificationRepository
    {
        private readonly AppDbContext _context;
        public EmailVerificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmailVerification?> GetByTokenAsync(string token)
        {
            return await _context.EmailVerifications
                .FirstOrDefaultAsync(ev => ev.Token == token);
        }

        public async Task AddAsync(EmailVerification email)
        {
            await _context.EmailVerifications.AddAsync(email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Delete(EmailVerification email)
        {
            _context.EmailVerifications.Remove(email);
        }

        public Task<IEnumerable<EmailVerification>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EmailVerification?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(EmailVerification entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(EmailVerification entity)
        {
            throw new NotImplementedException();
        }
    }
}
