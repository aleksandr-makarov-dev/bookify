using Bookify.Modules.Users.Application.Abstract;
using Bookify.Modules.Users.Domain;
using Bookify.Modules.Users.Infrastructure.Data;
using Bookify.Modules.Users.Infrastructure.Identity;
using Bookify.Modules.Users.Infrastructure.PublicApi;
using Bookify.Modules.Users.PublicApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Modules.Users.Infrastructure;

public static class DependencyInjection
{
    public static void AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "users"));
        });

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders()
            .AddPasswordlessLoginTokenProvider();

        services.AddTransient<ITokenProvider, TokenProvider>();
        services.AddScoped<IUserProvider, UserProvider>();

        services.AddScoped<IUsersApi, UsersApi>();
    }
}