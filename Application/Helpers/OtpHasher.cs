using BCrypt.Net;

public static class OtpHasher
{
    public static string HashOtp(string otp)
    {
        return BCrypt.Net.BCrypt.HashPassword(otp);
    }

    public static bool VerifyOtp(string enteredOtp, string hashedOtp)
    {
        return BCrypt.Net.BCrypt.Verify(enteredOtp, hashedOtp);
    }
}
