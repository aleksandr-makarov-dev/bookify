using Bookify.Modules.Scheduling.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Scheduling.Application.Abstract;

public interface ISchedulingDataProvider
{
    DbSet<EventType> EventTypes { get; }
    DbSet<AvailabilityRule> AvailabilityRules { get; }
    DbSet<DateOverride> DateOverrides { get; }
    DbSet<BookedSlot> BookedSlots { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}