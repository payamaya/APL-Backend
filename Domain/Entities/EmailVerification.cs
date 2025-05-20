
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class EmailVerification: BaseVerificationEntity
    {
        public string Token { get; set; } = null!;      // GUID or secure random string
    }
}
