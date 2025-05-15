
using Domain.Entities;
using Domain.Entities.Base;

namespace Domain.Interfaces
{
    public interface IOtpCodeRepository : IBaseRepository<OtpCode>
    {
        Task<OtpCode?> GetLatestValidOtpByEmailAsync(string email);
    }

}
