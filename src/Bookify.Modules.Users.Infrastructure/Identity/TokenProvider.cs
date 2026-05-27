using System.Security.Claims;
using System.Text;
using Bookify.Modules.Users.Application.Abstract;
using Bookify.Modules.Users.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bookify.Modules.Users.Infrastructure.Identity;

public class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string CreateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(user.Email);

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JsonWebToken:SecretKey"]!));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.ZoneInfo, user.TimeZone)
        ];

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("JsonWebToken:Expiration")),
            SigningCredentials = credentials,
            Issuer = configuration["JsonWebToken:Issuer"],
            Audience = configuration["JsonWebToken:Audience"],
        };


        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }
}