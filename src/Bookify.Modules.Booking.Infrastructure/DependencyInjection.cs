using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Modules.Booking.Infrastructure;

public static class DependencyInjection
{
    public static void AddBookingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "booking"));
        });

        services.AddScoped<IBookingDataProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<BookingDbContext>());
    }
}