namespace Bookify.Application.Authentication;

public interface IUserProvider
{
    public Guid UserId { get; }
    public string Email { get; }
    public string TimeZone { get; }
}