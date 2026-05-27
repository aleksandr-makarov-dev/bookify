using System.Text;
using Bookify.Modules.Users.Domain;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Modules.Users.Application.Users;

public class RequestLoginLinkResult
{
    public string Email { get; init; }
    public string LoginToken { get; init; }
}

public class RequestLoginLinkCommand : IRequest<ErrorOr<RequestLoginLinkResult>>
{
    public string Email { get; init; }
}

public class RequestLoginLinkValidator : AbstractValidator<RequestLoginLinkCommand>
{
    public RequestLoginLinkValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();
    }
}

public class RequestLoginLinkHandler(UserManager<User> userManager)
    : IRequestHandler<RequestLoginLinkCommand, ErrorOr<RequestLoginLinkResult>>
{
    public async Task<ErrorOr<RequestLoginLinkResult>> Handle(RequestLoginLinkCommand request,
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

        var rawSignInToken = await userManager.GenerateUserTokenAsync(user, "PasswordlessLoginProvider", "login");
        var encodedSignInToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawSignInToken));

        return new RequestLoginLinkResult
        {
            Email = request.Email,
            LoginToken = encodedSignInToken
        };
    }
}