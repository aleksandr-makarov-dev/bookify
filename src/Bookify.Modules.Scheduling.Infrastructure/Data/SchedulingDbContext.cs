using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Scheduling.Infrastructure.Data;

internal class SchedulingDbContext(DbContextOptions<SchedulingDbContext> options)
    : DbContext(options), ISchedulingDataProvider
{
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<AvailabilityRule> AvailabilityRules { get; set; }
    public DbSet<DateOverride> DateOverrides { get; set; }
    public DbSet<BookedSlot> BookedSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("scheduling");

        builder.Entity<EventType>(options =>
        {
            options.ToTable("EventTypes");

            options.HasKey(e => e.Id);

            options.Property(e => e.Title)
                .HasMaxLength(128)
                .IsRequired();

            options.Property(e => e.Description)
                .HasMaxLength(1024);

            options.Property(e => e.TimeZone)
                .HasMaxLength(64)
                .IsRequired();

            options.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsRequired();

            options.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");
        });

        builder.Entity<AvailabilityRule>(options =>
        {
            options.ToTable("AvailabilityRules");

            options.HasKey(e => e.Id);

            options.HasIndex(e => new { e.EventTypeId, e.DayOfWeek })
                .IsUnique();

            options.HasOne<EventType>()
                .WithMany(e=>e.AvailabilityRules)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<DateOverride>(options =>
        {
            options.ToTable("DateOverrides");

            options.HasKey(e => e.Id);

            options.HasIndex(e => new { e.EventTypeId, e.Date })
                .IsUnique();

            options.HasOne<EventType>()
                .WithMany(e=>e.DateOverrides)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<BookedSlot>(options =>
        {
            options.ToTable("BookedSlots");

            options.HasKey(e => e.Id);

            options.HasOne<EventType>()
                .WithMany(e=>e.BookedSlots)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}