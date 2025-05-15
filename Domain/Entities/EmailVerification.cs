
namespace Domain.Entities.Base
{
    public class EmailVerification: BaseVerificationEntity
    {
        public string Token { get; set; } = null!;      // GUID or secure random string
    }
}
