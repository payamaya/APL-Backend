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
            var verification = await _iEmail.GetByTokenAsync(token)
                ?? throw new AppException("Invalid email verification token.");

            if (verification.IsUsed)
                throw new AppException("This email token has already been used.");

            if (verification.ExpiresAt < DateTime.UtcNow)
                throw new AppException("This email token has expired.");

            var user = await _repos.Users.GetByIdAsync(verification.UserId)
                ?? throw new AppException("User associated with this token was not found.");

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
            var body = $"<p>Use this code to verify your account: <strong>{otpCode}</strong></p> It expires at {otp.ExpiresAt}.";
            await _emailService.SendEmailAsync(user.Email, subject, body);

            verification.IsUsed = true;

            await _repos.Users.SaveChangesAsync();
            await _otpCodeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repos.Users.FindByEmailAsync(dto.Email)
                ?? throw new UnauthorizedAccessException("Invalid credentials.");

            try
            {
                if (!_passwordHasher.VerifyPassword(user, user.Password, dto.Password))
                    throw new UnauthorizedAccessException("Invalid credentials.");
            }
            catch (SaltParseException ex)
            {
                // TODO: Log error
                throw new AppException("Password hashing failed. Please contact support.");
            }

            if (!user.EmailConfirmed)
                throw new AppException("Please confirm your email before logging in.");

            if (!user.IsOtpVerified)
                throw new AppException("Please verify OTP before logging in.");

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
            var existing = await _repos.Users.FindByEmailAsync(dto.Email);
            if (existing != null)
                throw new AppException("Email is already in use.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                    IsOtpVerified = false,
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

                var token = JwtHelper.GenerateToken(user, _config);

                var email = new EmailVerification
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                await _iEmail.AddAsync(email);
                await _repos.Users.SaveChangesAsync();

                var confirmUrl = $"{"http://localhost:3000"}/auth/confirm-email?token={token}";
                await _emailService.SendEmailAsync(dto.Email, "Confirm your account", $"Click <a href='{confirmUrl}'>here</a> to confirm your email.");

                await transaction.CommitAsync();
                return user.Id;
            }
            catch
            {
                if (transaction.GetDbTransaction().Connection != null)
                    await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SendOtpAsync(string email)
        {
            var user = await _repos.Users.FindByEmailAsync(email)
                ?? throw new AppException("User not found.");

            if (!user.EmailConfirmed)
                throw new AppException("Email is not confirmed.");

            var code = OtpHasher.GenerateOtp();
            var hashedOtp = OtpHasher.HashOtp(code);

            var otp = new OtpCode
            {
                Id = user.Id,
                Email = user.Email,
                Code = hashedOtp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _otpCodeRepository.AddAsync(otp);
            await _otpCodeRepository.SaveChangesAsync();

            await _emailService.SendEmailAsync(email, "Your OTP Code", $"Your one-time code is <b>{code}</b>. It expires in 10 minutes.");
        }

        public async Task<Guid> SetYourPassword(UserDto dto)
        {
            var user = await _repos.Users.FindByEmailAsync(dto.Email)
                ?? throw new AppException("User not found.");

            if (!user.EmailConfirmed)
                throw new AppException("Email must be confirmed before setting password.");

            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            await _repos.Users.SaveChangesAsync();

            return user.Id;
        }

        public async Task<(bool Success, string Message)> SetPasswordAsync(string password)
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return (false, "User email not found in token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return (false, "User not found");

            if (!user.EmailConfirmed)
                return (false, "Email not confirmed");

            if (!user.IsOtpVerified)
                return (false, "OTP not verified");

            user.Password = _passwordHasher.HashPassword(user, password);
            _context.Users.Update(user);
            var saved = await _context.SaveChangesAsync();

            return (saved > 0, saved > 0 ? "Password updated successfully" : "Failed to update password");
        }

        public async Task<bool> VerifyOtpAsync(string email, string code)
        {
            var otp = await _otpCodeRepository.GetLatestValidOtpByEmailAsync(email);
            if (otp == null)
                return false;

            if (OtpHasher.VerifyOtp(code, otp.Code) && otp.ExpiresAt > DateTime.UtcNow && !otp.IsUsed)
            {
                var user = await _repos.Users.FindByEmailAsync(email);
                if (user == null)
                    return false;

                user.IsOtpVerified = true;
                otp.IsUsed = true;

                await _otpCodeRepository.SaveChangesAsync();
                await _repos.Users.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }

}
