using Microsoft.AspNetCore.Identity;

namespace Bookify.Modules.Users.Infrastructure.Identity;

public class PasswordlessLoginTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public PasswordlessLoginTokenProviderOptions()
    {
        Name = "PasswordlessLoginTokenProvider";
        TokenLifespan = TimeSpan.FromSeconds(30);
    }
}