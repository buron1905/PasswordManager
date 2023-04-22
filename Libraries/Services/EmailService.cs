using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Models;
using Services.Abstraction;
using Services.Abstraction.Exceptions;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration? _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration>? emailConfiguration = null)
        {
            _emailConfiguration = emailConfiguration?.Value;
        }

        public void Send(string from, string password, string smtpServer, int port, string to, string subject, string text, bool isHtml = true)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain) { Text = text };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(smtpServer, port, SecureSocketOptions.Auto);
                smtp.Authenticate(from, password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        public void SendRegistrationEmail(string recipientEmailAddress, string emailConfirmationToken)
        {
            if (_emailConfiguration == null)
                throw new AppException("Server error. Email configuration is not set.");

            Send(_emailConfiguration.From, _emailConfiguration.Password, _emailConfiguration.SmtpServer, _emailConfiguration.Port,
                recipientEmailAddress, "Confirm registration",
                $"https://password-manager-client.azurewebsites.net/email-confirmation/{recipientEmailAddress}/{emailConfirmationToken}");
        }
    }
}
