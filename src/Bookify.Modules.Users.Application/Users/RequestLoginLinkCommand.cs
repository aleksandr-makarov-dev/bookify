using System.Text;
using Bookify.Modules.Users.Domain;
using Bookify.Modules.Users.IntegrationEvents;
using ErrorOr;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Modules.Users.Application.Users;

public sealed record RequestLoginLinkCommand(string Email) : IRequest<ErrorOr<Success>>;

public class RequestLoginLinkValidator : AbstractValidator<RequestLoginLinkCommand>
{
    public RequestLoginLinkValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();
    }
}

public class RequestLoginLinkHandler(UserManager<User> userManager, IBus bus)
    : IRequestHandler<RequestLoginLinkCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(RequestLoginLinkCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Error.Unauthorized(description: "User does not exist");
        }

        var isEmailVerified = await userManager.IsEmailConfirmedAsync(user);

        if (!isEmailVerified)
        {
            return Error.Unauthorized(description: "Email not confirmed");
        }

        var rawLoginToken = await userManager.GenerateUserTokenAsync(user, "PasswordlessLoginProvider", "login");
        var encodedLoginToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawLoginToken));

        await bus.Publish(new LoginRequestedIntegrationEvent(request.Email, encodedLoginToken), cancellationToken);

        return Result.Success;
    }
}