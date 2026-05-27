namespace Bookify.Modules.Users.Application.Abstract;

public interface IUserProvider
{
    public Guid UserId { get; }
    public string Email { get; }
    public string TimeZone { get; }
}