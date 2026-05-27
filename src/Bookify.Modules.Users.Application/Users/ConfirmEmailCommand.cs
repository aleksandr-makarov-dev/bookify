using System.Text;
using Bookify.Modules.Users.Domain;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Modules.Users.Application.Users;

public class ConfirmEmailCommand : IRequest<ErrorOr<Success>>
{
    public string Email { get; init; }
    public string Token { get; init; }
}

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(e => e.Token)
            .NotEmpty();
    }
}

public class ConfirmEmailCommandHandler(UserManager<User> userManager)
    : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Error.Unauthorized(description: "Invalid email or token");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var identityResult = await userManager.ConfirmEmailAsync(user, decodedToken);

        if (!identityResult.Succeeded)
        {
            return Error.Unauthorized(description: "Invalid email or token");
        }

        await userManager.UpdateSecurityStampAsync(user);

        return Result.Success;
    }
}