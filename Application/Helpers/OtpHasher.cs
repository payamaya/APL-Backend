public static class OtpHasher
{
    public static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();       
    }
    public static string HashOtp(string otp)
    {
        return BCrypt.Net.BCrypt.HashPassword(otp);
    }

    public static bool VerifyOtp(string enteredOtp, string hashedOtp)
    {
        return BCrypt.Net.BCrypt.Verify(enteredOtp, hashedOtp);
    }
}
