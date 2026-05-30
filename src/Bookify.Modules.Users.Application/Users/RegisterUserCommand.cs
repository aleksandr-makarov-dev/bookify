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

public sealed record RegisterUserCommand(string Name, string Email, string TimeZone) : IRequest<ErrorOr<Success>>;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty();

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.TimeZone)
            .NotEmpty();
    }
}

public class RegisterUserCommandHandler(UserManager<User> userManager, IBus bus)
    : IRequestHandler<RegisterUserCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = new User(request.Name, request.Email, request.TimeZone);

        var identityResult = await userManager.CreateAsync(user);

        if (!identityResult.Succeeded)
        {
            return Error.Conflict(description: identityResult.Errors.First().Description);
        }

        var rawEmailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedEmailConfirmationToken =
            WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawEmailConfirmationToken));

        await bus.Publish(new UserRegisteredIntegrationEvent(request.Email, encodedEmailConfirmationToken),
            cancellationToken);

        return Result.Success;
    }
}