using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Booking.Infrastructure.Data;

internal class BookingDbContext(DbContextOptions<BookingDbContext> options)
    : DbContext(options), IBookingDataProvider
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("booking");

        builder.Entity<Order>(options =>
        {
            options.ToTable("Orders");

            options.HasKey(x => x.Id);

            options.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsRequired();

            options.Property(e => e.GuestEmail)
                .HasMaxLength(128)
                .IsRequired();

            options.Property(e => e.Message)
                .HasMaxLength(256);
        });

        builder.Entity<Payment>(options =>
        {
            options.ToTable("Payments");

            options.HasKey(x => x.Id);

            options.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsRequired();

            options.Property(e => e.ExternalPaymentId)
                .HasMaxLength(256)
                .IsRequired();

            options.HasOne<Order>()
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}