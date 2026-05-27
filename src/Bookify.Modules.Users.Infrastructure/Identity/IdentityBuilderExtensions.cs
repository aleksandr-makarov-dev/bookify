using Microsoft.AspNetCore.Identity;

namespace Bookify.Modules.Users.Infrastructure.Identity;

public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddPasswordlessLoginTokenProvider(this IdentityBuilder builder)
    {
        var userType = builder.UserType;
        var provider= typeof(PasswordlessLoginTokenProvider<>).MakeGenericType(userType);
        return builder.AddTokenProvider("PasswordlessLoginProvider", provider);
    }
}