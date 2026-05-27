using Bookify.Modules.Users.Domain;

namespace Bookify.Modules.Users.Application.Abstract;

public interface ITokenProvider
{
    string CreateToken(User user);
}