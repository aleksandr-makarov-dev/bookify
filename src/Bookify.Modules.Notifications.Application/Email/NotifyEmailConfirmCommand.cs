using Bookify.Modules.Notifications.Application.Abstract;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Bookify.Modules.Notifications.Application.Email;

public sealed record NotifyEmailConfirmCommand(
    string Email,
    string EmailConfirmationToken
) : IRequest<ErrorOr<Success>>;

public sealed class NotifyEmailConfirmCommandHandler(IEmailSender emailSender, IConfiguration configuration)
    : IRequestHandler<NotifyEmailConfirmCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        NotifyEmailConfirmCommand request,
        CancellationToken cancellationToken)
    {
        var emailConfirmationLink =
            $"{configuration["BaseUrl"]}/api/users/confirm-email?token={Uri.EscapeDataString(request.EmailConfirmationToken)}&email={Uri.EscapeDataString(request.Email)}";

        var body = $"""
                    <div>
                        <p>Hello,</p>
                        <p>Thank you for registering.</p>
                        <p>Please confirm your email address by clicking the link below:</p>
                        <a href="{emailConfirmationLink}">{emailConfirmationLink}</a>
                        <p>If you did not create an account, you can safely ignore this email.</p>
                        <p>Best regards</p>
                    </div>
                    """;

        await emailSender.SendEmailAsync(
            request.Email,
            "Confirm Your Email Address",
            body);

        return Result.Success;
    }
}