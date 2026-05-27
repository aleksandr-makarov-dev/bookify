using System.Security.Claims;

namespace Bookify.Modules.Users.Infrastructure.Identity;

public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var guid)
            ? guid
            : throw new InvalidOperationException("User not authenticated.");
    }
}