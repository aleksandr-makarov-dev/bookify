using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Infrastructure.Availability;
using Bookify.Modules.Scheduling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Modules.Scheduling.Infrastructure;

public static class DependencyInjection
{
    public static void AddSchedulingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SchedulingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "scheduling"));
        });

        services.AddScoped<ISchedulingDataProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<SchedulingDbContext>());

        services.AddScoped<IAvailabilityProvider, AvailabilityProvider>();
    }
}