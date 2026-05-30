using Bookify.Modules.Scheduling.Application.Abstract.Models;

namespace Bookify.Modules.Scheduling.Application.Abstract;

public interface IAvailabilityProvider
{
    Task<Dictionary<DateOnly, List<AvailableSlotDto>>> GetAvailableSlotsAsync(Guid eventTypeId, DateOnly startDate,
        DateOnly endDate,
        string timeZone, CancellationToken ct = default);
}