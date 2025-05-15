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
using Microsoft.Extensions.Configuration;

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
        public AuthService(AppDbContext context, IRepositoryWrapper repos, IConfiguration config, IEmailVerificationRepository emailVerificationRepository, IEmailService emailService, IOtpCodeRepository otpCodeRepository)
    {
        _context = context;
        _repos = repos;
        _config = config;
        _iEmail = emailVerificationRepository;
        _emailService = emailService;
        _otpCodeRepository = otpCodeRepository;
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

            // 3. Mark token as used
            verification.IsUsed = true;

            await _repos.Users.SaveChangesAsync(); // assumes shared context with verification repository

            return true;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _repos.Users.FindByEmailAsync(dto.Email);

            if (user == null || !PasswordHasher.Verify(dto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
            // Optional: Enforce email confirmation
            if (!user.EmailConfirmed)
            {
                throw new Exception("Please confirm your email before logging in.");
            }

            // Check if OTP verification is required
            if (!user.IsOtpVerified) // <- Add this flag on the user entity
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
                    UserId = Guid.NewGuid(),
                    Email = dto.Email,
                    Password = PasswordHasher.Hash(dto.Password),
                    Role = dto.Role,
                    EmailConfirmed = false,
                    IsOtpVerified = false, // <- Add this flag on the user entity
                    CreatedAt = DateTime.UtcNow
                };

                await _repos.Users.AddAsync(user);
                switch (dto.Role)
                {
                    case Role.Student:
                        await _repos.Students.AddAsync(new Student { UserId = user.UserId, Email = user.Email });
                        break;
                    case Role.Teacher:
                        await _repos.Teachers.AddAsync(new Teacher { UserId = user.UserId, Email = user.Email });
                        break;
                }

                // 3. Generate and store token
                //var token = Guid.NewGuid().ToString("N"); // 32-char alphanumeric string
                var token = JwtHelper.GenerateToken(user, _config); // 529-char JWT token

                var email = new EmailVerification
                {
                    Id = Guid.NewGuid(),
                    UserId = user.UserId,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                await _iEmail.AddAsync(email);
                await _repos.Users.SaveChangesAsync();

                // 4. Build confirmation URL
                var confirmUrl = $"{"http://localhost:3000"}/confirm?token={token}";

                // 5. Send confirmation email
                var subject = "Confirm your account";
                var body = $"<h1>Welcome</h1> Click <a href='{confirmUrl}'>here</a> to confirm your email.";

                await _emailService.SendEmailAsync(dto.Email, subject, body);
                await transaction.CommitAsync();
                return user.UserId;
            }
            catch
            {
                await transaction.RollbackAsync();
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
            var code = new Random().Next(100000, 999999).ToString();
            string hashedOtp = OtpHasher.HashOtp(code);

            // 3. Store in OtpCodes table
            var otp = new OtpCode
            {
                UserId = user.UserId,
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
