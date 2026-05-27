using System.Security.Claims;
using Bookify.Modules.Users.Application.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Bookify.Modules.Users.Infrastructure.Identity;

public class UserProvider(IHttpContextAccessor context) : IUserProvider
{
    public Guid UserId => context.HttpContext?.User.GetUserId() ??
                          throw new InvalidOperationException("User not authenticated.");

    public string Email => context.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ??
                           throw new InvalidOperationException("User not authenticated.");

    public string TimeZone => context.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.ZoneInfo) ??
                              throw new InvalidOperationException("User not authenticated.");
}