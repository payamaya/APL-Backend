
namespace Domain.Entities.Base
{
    public class OtpCode: BaseVerificationEntity
    {
        public string Code { get; set; } = null!;       // e.g. 6 digits
        public string Email { get; set; } = string.Empty; // Optional, if you want to store the email
    }
}
