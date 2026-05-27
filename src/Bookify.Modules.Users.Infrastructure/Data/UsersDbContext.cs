using Bookify.Modules.Users.Application.Abstract;
using Bookify.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Users.Infrastructure.Data;

internal class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("users");

        builder.Entity<User>(options =>
        {
            options.Property(e => e.TimeZone)
                .HasMaxLength(128)
                .IsRequired();

            options.Property(e => e.Name)
                .HasMaxLength(128)
                .IsRequired();
        });
    }
}