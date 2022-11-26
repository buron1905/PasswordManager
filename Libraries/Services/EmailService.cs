using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Services.Abstraction;

namespace Services
{
    public class EmailService : IEmailService
    {           
        public void Send(string from, string to, string subject, string text, bool isHtml = true)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain) { Text = text };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.ethereal.com", 587, SecureSocketOptions.StartTls);
                //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("guadalupe93@ethereal.email", "SfcW9SJGHuKmSvsAhB");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
