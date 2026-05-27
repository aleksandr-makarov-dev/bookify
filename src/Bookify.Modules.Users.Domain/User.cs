using Microsoft.AspNetCore.Identity;

namespace Bookify.Modules.Users.Domain;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string TimeZone { get; set; }

    private User()
    {
    }

    public User(string name, string email, string timeZone)
    {
        Name = name;
        Email = email;
        UserName = email;
        TimeZone = timeZone;
    }
}