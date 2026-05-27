using Bookify.Modules.Users.Domain;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bookify.Modules.Users.Application.Users;

public class UserResult
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string TimeZone { get; init; }
}

public class GetUserQuery : IRequest<ErrorOr<UserResult>>
{
    public Guid Id { get; init; }
}

internal sealed class GetUserQueryHandler(UserManager<User> userManager)
    : IRequestHandler<GetUserQuery, ErrorOr<UserResult>>
{
    public async Task<ErrorOr<UserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        return new UserResult();
    }
}