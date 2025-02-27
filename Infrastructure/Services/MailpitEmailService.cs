using Application.Common;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class MailpitEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public MailpitEmailService()
        {
            _smtpClient = new SmtpClient("localhost", 1025)
            {
                EnableSsl = false,
                Credentials = null
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("no-reply@yourapp.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
