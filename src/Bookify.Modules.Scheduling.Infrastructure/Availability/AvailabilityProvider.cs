using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Application.Abstract.Models;
using Bookify.Modules.Scheduling.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Scheduling.Infrastructure.Availability;

public class AvailabilityProvider(ISchedulingDataProvider schedulingDataProvider) : IAvailabilityProvider
{
    public async Task<Dictionary<DateOnly, List<AvailableSlotDto>>> GetAvailableSlotsAsync(Guid eventTypeId,
        DateOnly startDate,
        DateOnly endDate, string timeZone, CancellationToken ct = default)
    {
        var eventType = await schedulingDataProvider.EventTypes
            .AsNoTracking()
            .Where(e => e.Id == eventTypeId)
            .Include(e => e.AvailabilityRules)
            .Include(e => e.DateOverrides)
            .Include(e => e.BookedSlots)
            .FirstOrDefaultAsync(ct);

        if (eventType is null)
        {
            return new Dictionary<DateOnly, List<AvailableSlotDto>>();
        }

        var result = new Dictionary<DateOnly, List<AvailableSlotDto>>();

        var overridesByDate = eventType.DateOverrides
            .ToDictionary(x => x.Date);

        var rulesByDayOfWeek = eventType.AvailabilityRules
            .ToDictionary(x => x.DayOfWeek);

        var bookedByDate = eventType.BookedSlots
            .GroupBy(x => DateOnly.FromDateTime(x.StartDateTime))
            .ToDictionary(g => g.Key, g => g.ToList());

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var bookedSlots = bookedByDate.TryGetValue(date, out var value) ? value : [];

            if (overridesByDate.TryGetValue(date, out var ov))
            {
                if (ov.IsUnavailable)
                {
                    continue;
                }

                result[date] = GenerateAvailableSlots(date, ov.StartTime, ov.EndTime, eventType.Duration, timeZone,
                    eventType.TimeZone, bookedSlots);

                continue;
            }

            if (!rulesByDayOfWeek.TryGetValue(date.DayOfWeek, out var rule))
            {
                continue;
            }

            result[date] = GenerateAvailableSlots(date, rule.StartTime, rule.EndTime, eventType.Duration, timeZone,
                eventType.TimeZone, bookedSlots);
        }

        return result;
    }

    private List<AvailableSlotDto> GenerateAvailableSlots(DateOnly date, TimeOnly startTime, TimeOnly endTime,
        int duration, string clientTimeZone, string hostTimeZone, List<BookedSlot> bookedSlots)
    {
        var hostTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(hostTimeZone);
        var clientTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone);

        var slots = new List<AvailableSlotDto>();

        for (var time = startTime; time.AddMinutes(duration) <= endTime; time = time.AddMinutes(duration))
        {
            var hostStartDateTime = DateTime.SpecifyKind(new DateTime(date, time), DateTimeKind.Unspecified);
            var hostEndDateTime =
                DateTime.SpecifyKind(new DateTime(date, time.AddMinutes(duration)), DateTimeKind.Unspecified);

            var utcStartDateTime = TimeZoneInfo.ConvertTimeToUtc(hostStartDateTime, hostTimeZoneInfo);
            var utcEndDateTime = TimeZoneInfo.ConvertTimeToUtc(hostEndDateTime, hostTimeZoneInfo);

            var isOverlapping = bookedSlots.Any(slot =>
                utcStartDateTime < slot.EndDateTime &&
                utcEndDateTime > slot.StartDateTime);

            if (isOverlapping)
            {
                continue;
            }

            var clientStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcStartDateTime, clientTimeZoneInfo);
            var clientEndDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcEndDateTime, clientTimeZoneInfo);

            slots.Add(new AvailableSlotDto
            {
                StartDateTime = clientStartDateTime,
                EndDateTime = clientEndDateTime,
            });
        }

        return slots;
    }
}