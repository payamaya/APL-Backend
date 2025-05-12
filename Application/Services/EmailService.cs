using System.Net.Mail;
using System.Net;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _smtp;
        public EmailService(IConfiguration config)
        {
            _config = config;
            _smtp = new SmtpClient(
                _config["Email:SmtpHost"],
                int.Parse(_config["Email:SmtpPort"]))
            {
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                ),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var mail = new MailMessage(_config["Email:FromAddress"], to, subject, htmlContent)
            {
                IsBodyHtml = true
            };
            await _smtp.SendMailAsync(mail);
        }
    }

}
