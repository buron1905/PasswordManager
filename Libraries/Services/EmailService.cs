using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Models;
using Services.Abstraction;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration? _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration>? emailConfiguration = null)
        {
            _emailConfiguration = emailConfiguration?.Value;
        }

        public void Send(string from, string to, string subject, string text, bool isHtml = true)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain) { Text = text };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
                //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailConfiguration.From, _emailConfiguration.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        public void SendRegistrationEmail(string recipientEmailAddress, string emailConfirmationToken)
        {
            Send(_emailConfiguration.From, recipientEmailAddress, "Confirm registration", $"https://localhost:5001/email-confirmation/{recipientEmailAddress}/{emailConfirmationToken}");
        }
    }
}
