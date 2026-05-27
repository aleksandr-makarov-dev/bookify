using Microsoft.AspNetCore.Identity;

namespace Bookify.Modules.Users.Domain;

public class Role : IdentityRole<Guid>
{
    public static readonly string Administrator = nameof(Administrator);
    public static readonly string Member = nameof(Member);

    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}