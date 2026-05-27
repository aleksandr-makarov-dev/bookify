using Bookify.Modules.Users.Application.Users;
using Bookify.Modules.Users.PublicApi;
using MediatR;

namespace Bookify.Modules.Users.Infrastructure.PublicApi;

internal sealed class UsersApi(IMediator mediator) : IUsersApi
{
    public async Task<UserDto?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetUserQuery { Id = userId }, cancellationToken);

        if (result.IsError)
        {
            return null;
        }

        var user = new UserDto
        {
            Id = result.Value.Id,
            Email = result.Value.Email,
            Name = result.Value.Name,
            TimeZone = result.Value.TimeZone,
        };

        return user;
    }
}