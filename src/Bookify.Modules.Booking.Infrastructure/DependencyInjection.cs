using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Infrastructure.Data;
using Bookify.Modules.Booking.Infrastructure.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Stripe.Checkout;

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

        services.AddScoped<IPaymentProvider, StripePaymentProvider>();

        services.AddSingleton<StripeClient>(_ => new StripeClient(configuration["Stripe:SecretKey"]));
        services.AddScoped<SessionService>(serviceProvider =>
            new SessionService(serviceProvider.GetRequiredService<StripeClient>()));
    }
}