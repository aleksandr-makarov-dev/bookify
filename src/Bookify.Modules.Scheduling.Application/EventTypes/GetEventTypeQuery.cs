using Bookify.Modules.Scheduling.Application.Abstract;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Scheduling.Application.EventTypes;

public class EventTypeDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int Duration { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
}

public class GetEventTypeQuery : IRequest<ErrorOr<EventTypeDto>>
{
    public Guid Id { get; init; }
}

public class GetEventTypeQueryHandler(ISchedulingDataProvider schedulingDataProvider)
    : IRequestHandler<GetEventTypeQuery, ErrorOr<EventTypeDto>>
{
    public async Task<ErrorOr<EventTypeDto>> Handle(GetEventTypeQuery request, CancellationToken cancellationToken)
    {
        var eventType = await schedulingDataProvider.EventTypes
            .AsNoTracking()
            .Where(e => e.Id == request.Id)
            .Select(e => new EventTypeDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Duration = e.Duration,
                Price = e.Price,
                Currency = e.Currency,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (eventType is null)
        {
            return Error.NotFound(description: "Event not found");
        }

        return eventType;
    }
}