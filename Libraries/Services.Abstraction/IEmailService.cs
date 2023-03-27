namespace Services.Abstraction
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string text, bool isHtml = true);
        void SendRegistrationEmail(string recipientEmailAddress, string emailConfirmationToken);
    }
}
