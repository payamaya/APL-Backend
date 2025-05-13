using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmailVerificationRepository : IBaseRepository<EmailVerification>
    {
        void Delete(EmailVerification email);
        Task<EmailVerification?> GetByTokenAsync(string token);

    }
}
