using Bookify.Domain;
using Bookify.Modules.Scheduling.Application.Abstract;
using Bookify.Modules.Scheduling.Domain;
using ErrorOr;
using MediatR;

namespace Bookify.Modules.Scheduling.Application.EventTypes;

public class AvailabilityRuleDto
{
    public DayOfWeek DayOfWeek { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}

public class DateOverrideDto
{
    public DateOnly Date { get; init; }
    public bool IsUnavailable { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}

public class CreateEventTypeCommand : IRequest<ErrorOr<Guid>>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string TimeZone { get; init; }
    public int Duration { get; init; }
    public decimal Price { get; init; }
    public Guid OwnerId { get; init; }
    public List<AvailabilityRuleDto> AvailabilityRules { get; init; } = [];
    public List<DateOverrideDto> DateOverrides { get; init; } = [];
}

public class CreateEventTypeCommandHandler(ISchedulingDataProvider schedulingDataProvider)
    : IRequestHandler<CreateEventTypeCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
    {
        var eventTypeId = Guid.NewGuid();

        var eventType = new EventType
        {
            Id = eventTypeId,
            Title = request.Title,
            Description = request.Description,
            TimeZone = request.TimeZone,
            Duration = request.Duration,
            Price = request.Price,
            Currency = Currency.EUR,
            OwnerId = request.OwnerId,
        };

        var availabilityRuleList = request.AvailabilityRules.Select(rule => new AvailabilityRule
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                DayOfWeek = rule.DayOfWeek,
                StartTime = rule.StartTime,
                EndTime = rule.EndTime,
            })
            .ToList();

        eventType.AvailabilityRules.AddRange(availabilityRuleList);

        var dateOverrideList = request.DateOverrides.Select(date => new DateOverride
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                Date = date.Date,
                IsUnavailable = date.IsUnavailable,
                StartTime = date.StartTime,
                EndTime = date.EndTime,
            })
            .ToList();

        eventType.DateOverrides.AddRange(dateOverrideList);

        schedulingDataProvider.EventTypes.Add(eventType);

        await schedulingDataProvider.SaveChangesAsync(cancellationToken);

        return eventTypeId;
    }
}