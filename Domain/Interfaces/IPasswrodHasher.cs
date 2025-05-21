using BCrypt.Net;
using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    
public interface IPasswordHasher<T>
{
    string HashPassword(T user, string password);
    bool VerifyPassword(T user, string hashedPassword, string providedPassword);
}

public class PasswordHasher : IPasswordHasher<User>
{

        public string HashPassword(User user, string password)
        {
            // Add work factor and ensure consistent hashing
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }


        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch (SaltParseException)
            {
                // Log this error for debugging
                Console.WriteLine("Invalid salt version encountered");
                return false;
            }
        }
    }
}