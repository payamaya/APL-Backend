using Application.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validators
{
    public class FileDtoValidator : AbstractValidator<FileDto>
    {
        public FileDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(file => file != null && file.Length > 0).WithMessage("File must not be empty.")
                .Must(file => IsAllowedContentType(file)).WithMessage("Unsupported file type.");

            RuleFor(x => x.ActivityId)
                .NotEmpty().WithMessage("Activity ID is required.");
        }

        private bool IsAllowedContentType(IFormFile? file)
        {
            if (file == null) return false;
            var allowedContentTypes = new[] {
                "application/pdf", "image/jpeg", "image/png", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
            return allowedContentTypes.Contains(file.ContentType);
        }
    }
}
