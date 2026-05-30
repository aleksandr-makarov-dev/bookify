using Bookify.Modules.Notifications.Application.Abstract;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Bookify.Modules.Notifications.Application.Email;

public sealed record NotifyLoginConfirmCommand(string Email, string LoginToken) : IRequest<ErrorOr<Success>>;

internal sealed class NotifyLoginConfirmCommandHandler(IEmailSender emailSender, IConfiguration configuration)
    : IRequestHandler<NotifyLoginConfirmCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(NotifyLoginConfirmCommand request, CancellationToken cancellationToken)
    {
        var loginLink =
            $"{configuration["BaseUrl"]}/api/users/login-link/confirm?token={Uri.EscapeDataString(request.LoginToken)}&email={Uri.EscapeDataString(request.Email)}";

        var body = $"""
                    <div>
                        <p>Hello,</p>
                        <p>You requested to sign in.</p>
                        <p>Please click the link below to sign in:</p>
                        <a href="{loginLink}">{loginLink}</a>
                        <p>If you did not request this sign in link, you can safely ignore this email.</p>
                        <p>Best regards</p>
                    </div>
                    """;

        await emailSender.SendEmailAsync(
            request.Email,
            "Sign In Link",
            body);

        return Result.Success;
    }
}