using Application.DTOs;
using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class OtpValidator : AbstractValidator<OtpVerifyDto>
    {
        public OtpValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be exactly 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("OTP code must contain only numbers.");
        }
    }
}
