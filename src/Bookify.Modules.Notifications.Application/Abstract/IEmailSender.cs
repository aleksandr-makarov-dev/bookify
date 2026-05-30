namespace Bookify.Modules.Notifications.Application.Abstract;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}