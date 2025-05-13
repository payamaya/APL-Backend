
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOtpCodeRepository : IBaseRepository<OtpCode>
    {
        Task<OtpCode?> GetLatestValidOtpByEmailAsync(string email);
    }

}
