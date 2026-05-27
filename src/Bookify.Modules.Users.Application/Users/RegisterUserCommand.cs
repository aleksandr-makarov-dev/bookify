using System.Text;
using Bookify.Modules.Users.Domain;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Modules.Users.Application.Users;

public class RegisterUserResult
{
    public string Email { get; init; }
    public string EmailConfirmationToken { get; init; }
}

public class RegisterUserCommand : IRequest<ErrorOr<RegisterUserResult>>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string TimeZone { get; init; }
}

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

public class RegisterUserCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResult>>
{
    public async Task<ErrorOr<RegisterUserResult>> Handle(RegisterUserCommand request,
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

        return new RegisterUserResult
        {
            Email = request.Email,
            EmailConfirmationToken = encodedEmailConfirmationToken,
        };
    }
}