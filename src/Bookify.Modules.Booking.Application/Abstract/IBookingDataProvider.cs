using Bookify.Modules.Booking.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Booking.Application.Abstract;

public interface IBookingDataProvider
{
    DbSet<Order> Orders { get; }
    DbSet<Payment> Payments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}