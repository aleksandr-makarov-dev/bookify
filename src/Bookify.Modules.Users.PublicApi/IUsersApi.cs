namespace Bookify.Modules.Users.PublicApi;

public interface IUsersApi
{
    Task<UserDto?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}