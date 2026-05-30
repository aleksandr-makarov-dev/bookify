using System.Reflection;
using Bookify.Application.Authentication;
using Bookify.Infrastructure.Authentication;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration,
        Assembly[] assemblies)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(assemblies);

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], "/", host =>
                {
                    host.Username(configuration["RabbitMq:Username"]);
                    host.Password(configuration["RabbitMq:Password"]);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddAuthenticationInternal(configuration);

        services.AddScoped<IUserProvider, UserProvider>();
    }
}