using Bookify.Modules.Notifications.Application.Abstract;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Bookify.Modules.Notifications.Infrastructure.Email;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(configuration["Email:From"]!));

        message.To.Add(
            MailboxAddress.Parse(email));

        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = htmlMessage
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            configuration["Email:Host"]!,
            int.Parse(configuration["Email:Port"]!),
            MailKit.Security.SecureSocketOptions.None);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}