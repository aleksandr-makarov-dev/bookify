using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Application.Abstract.Models;
using MediatR;

namespace Bookify.Modules.Scheduling.Application.Availability;

public class GetAvailabilityQuery : IRequest<Dictionary<DateOnly, List<AvailableSlotDto>>>
{
    public Guid EventTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string TimeZone { get; set; }
}

public class GetAvailabilityQueryHandler(IAvailabilityProvider availabilityProvider)
    : IRequestHandler<GetAvailabilityQuery, Dictionary<DateOnly, List<AvailableSlotDto>>>
{
    public async Task<Dictionary<DateOnly, List<AvailableSlotDto>>> Handle(GetAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        var slots = await availabilityProvider.GetAvailableSlotsAsync(request.EventTypeId, request.StartDate,
            request.EndDate, request.TimeZone, cancellationToken);

        return slots;
    }
}