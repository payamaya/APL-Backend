using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Base;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Base;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;


namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IRepositoryWrapper _repos;
        private readonly IConfiguration _config;
        private readonly IEmailVerificationRepository _iEmail;
        private readonly IEmailService _emailService;
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(AppDbContext context, IRepositoryWrapper repos, IConfiguration config, IEmailVerificationRepository emailVerificationRepository, IEmailService emailService, IOtpCodeRepository otpCodeRepository, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _repos = repos;
            _config = config;
            _iEmail = emailVerificationRepository;
            _emailService = emailService;
            _otpCodeRepository = otpCodeRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            // 1. Find token and validate
            var verification = await _iEmail.GetByTokenAsync(token);
            if (verification == null)
                throw new Exception("Invalid token.");

            if (verification.IsUsed)
                throw new InvalidOperationException("Token has already been used.");

            if (verification.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Token has expired.");

            // 2. Confirm user's email
            var user = await _repos.Users.GetByIdAsync(verification.UserId);
            if (user == null)
                throw new Exception("User not found.");

            user.EmailConfirmed = true;

            var otpCode = OtpHasher.GenerateOtp();
            var otp = new OtpCode
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Code = OtpHasher.HashOtp(otpCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };
            await _otpCodeRepository.AddAsync(otp);
            var subject = "Your OTP Code";
            var body = $"<p>Use this code to verify your account: <strong>{otpCode}</strong></p> which will be expired at {otp.ExpiresAt}";
            await _emailService.SendEmailAsync(user.Email, subject, body);
            // 3. Mark token as used
            verification.IsUsed = true;

            await _repos.Users.SaveChangesAsync(); // assumes shared context with verification repository
            await _otpCodeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repos.Users.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Verify password with proper error handling
            try
            {
                if (!_passwordHasher.VerifyPassword(user, user.Password, dto.Password))
                {
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }
            }
            catch (SaltParseException ex)
            {
                // Log this specific error for debugging
                Console.WriteLine($"Password verification failed: {ex.Message}");
                throw new Exception("Authentication system error. Please contact support.");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Please confirm your email before logging in.");
            }

            if (!user.IsOtpVerified)
            {
                throw new Exception("OTP not verified. Please verify OTP to complete login.");
            }

            var token = JwtHelper.GenerateToken(user, _config);

            return new AuthResponseDto
            {
                Token = token,
                Role = user.Role.ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:ExpiresInMinutes"]))
            };
        }
        public async Task<Guid> RegisterWithEmailConfirmationAsync(UserDto dto)
        {
            // 1. Check for existing email
            var existing = await _repos.Users.FindByEmailAsync(dto.Email);
            if (existing != null)
                throw new ConflictException("Email already in use.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. Create user with EmailConfirmed = false
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Telephone = dto.Telephone,
                    Address = dto.Address,
                    Email = dto.Email,
                    Password = _passwordHasher.HashPassword(new User { Email = dto.Email }, dto.Password),
                    Role = dto.Role,
                    EmailConfirmed = false,
                    IsOtpVerified = false, // <- Add this flag on the user entity
                    CreatedAt = DateTime.UtcNow
                };

                await _repos.Users.AddAsync(user);
                switch (dto.Role)
                {
                    case Role.Student:
                        await _repos.Students.AddAsync(new Student
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Telephone = user.Telephone,
                            Address = user.Address,
                            CreatedAt = user.CreatedAt
                        });
                        break;
                    case Role.Teacher:
                        await _repos.Teachers.AddAsync(new Teacher
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Telephone = user.Telephone,
                            Address = user.Address,
                            CreatedAt = user.CreatedAt
                        });
                        break;
                }

                // 3. Generate and store token
                //var token = Guid.NewGuid().ToString("N"); // 32-char alphanumeric string
                var token = JwtHelper.GenerateToken(user, _config); // 529-char JWT token

                var email = new EmailVerification
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                await _iEmail.AddAsync(email);
                await _repos.Users.SaveChangesAsync();

                // 4. Build confirmation URL
                var confirmUrl = $"{"http://localhost:3000"}/auth/confirm-email?token={token}";

                // 5. Send confirmation email
                var subject = "Confirm your account";
                var body = $"<h1>Welcome</h1> Click <a href='{confirmUrl}'>here</a> to confirm your email.";

                await _emailService.SendEmailAsync(dto.Email, subject, body);
                await transaction.CommitAsync();
                return user.Id;
            }
            catch
            {
                if (transaction.GetDbTransaction().Connection != null)
                {
                    await transaction.RollbackAsync();
                }
                throw;
            }
        }

        public async Task SendOtpAsync(string email)
        {
            // 1. Find user and validate
            var user = await _repos.Users.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found.");
            if (!user.EmailConfirmed)
                throw new InvalidOperationException("Email is not confirmed.");

            // 2. Generate 6-digit OTP
            var code = OtpHasher.GenerateOtp();
            string hashedOtp = OtpHasher.HashOtp(code);

            // 3. Store in OtpCodes table
            var otp = new OtpCode
            {
                Id = user.Id,
                Email = user.Email,
                Code = hashedOtp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            if (_otpCodeRepository == null)
                throw new Exception("_otpCodeRepository is not injected.");

            if (otp == null)
                throw new Exception("OTP object is null.");

            await _otpCodeRepository.AddAsync(otp);
            await _otpCodeRepository.SaveChangesAsync();

            // 4. Send OTP email
            await _emailService.SendEmailAsync(
                email,
                "Your OTP Code",
                $"Your one-time code is <b>{code}</b>. It expires in 10 minutes."
            );
        }
        public async Task<Guid> SetYourPassword(UserDto dto)
        {
            var user = await _repos.Users.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found.");

            if (!user.EmailConfirmed)
                throw new Exception("Email must be confirmed before setting password.");

            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            await _repos.Users.SaveChangesAsync();

            return user.Id;
        }

        public async Task<(bool Success, string Message)> SetPasswordAsync(string password)
        {
            // Get email from JWT claims
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return (false, "User email not found in token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return (false, "User not found");

            if (!user.EmailConfirmed)
                return (false, "Please confirm your email first");

            if (!user.IsOtpVerified)
                return (false, "Please verify OTP first");

            user.Password = _passwordHasher.HashPassword(user, password);
            _context.Users.Update(user);

            var saved = await _context.SaveChangesAsync();

            return (saved > 0,
                saved > 0 ? "Password updated successfully" : "Failed to update password");
        }


        public async Task<bool> VerifyOtpAsync(string email, string code)
        {
            // 1. Lookup the most recent unused OTP for the user
            var otp = await _otpCodeRepository.GetLatestValidOtpByEmailAsync(email);
            if (otp == null)
                return false;

            // 2. Check if the code matches and hasn't expired
            if (OtpHasher.VerifyOtp(code, otp.Code) && otp.ExpiresAt > DateTime.UtcNow && !otp.IsUsed)
            {
                var user = await _repos.Users.FindByEmailAsync(email); // <- Add this line

                if (user == null)
                    return false;

                user.IsOtpVerified = true; // Make sure this property exists on the User entity
                otp.IsUsed = true;

                await _otpCodeRepository.SaveChangesAsync(); // You also need to save user changes
                await _repos.Users.SaveChangesAsync();     // <- Add this if you don't have a UnitOfWork

                return true;
            }

            // 3. If not valid
            return false;
        }

    }

}
