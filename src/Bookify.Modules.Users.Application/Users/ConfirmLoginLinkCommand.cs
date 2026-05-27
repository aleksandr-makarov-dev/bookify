using System.Text;
using Bookify.Modules.Users.Application.Abstract;
using Bookify.Modules.Users.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Modules.Users.Application.Users;

public class ConfirmLoginLinkResult
{
    public string Token { get; set; }
}

public class ConfirmLoginLinkCommand : IRequest<ErrorOr<ConfirmLoginLinkResult>>
{
    public string Email { get; init; }
    public string Token { get; init; }
}

public class ConfirmLoginLinkCommandHandler(UserManager<User> userManager, ITokenProvider tokenProvider)
    : IRequestHandler<ConfirmLoginLinkCommand, ErrorOr<ConfirmLoginLinkResult>>
{
    public async Task<ErrorOr<ConfirmLoginLinkResult>> Handle(ConfirmLoginLinkCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Error.Unauthorized(description: "Invalid email or token");
        }

        var decodedLoginToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var isLoginTokenValid =
            await userManager.VerifyUserTokenAsync(user, "PasswordlessLoginProvider", "login", decodedLoginToken);

        if (!isLoginTokenValid)
        {
            return Error.Unauthorized(description: "Invalid email or token");
        }

        var updateSecurityStampResult = await userManager.UpdateSecurityStampAsync(user);

        if (!updateSecurityStampResult.Succeeded)
        {
            return Error.Unauthorized(description: updateSecurityStampResult.Errors.First().Description);
        }

        var accessToken = tokenProvider.CreateToken(user);

        return new ConfirmLoginLinkResult
        {
            Token = accessToken,
        };
    }
}