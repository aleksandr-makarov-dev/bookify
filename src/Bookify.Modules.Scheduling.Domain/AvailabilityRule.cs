using Bookify.Domain;

namespace Bookify.Modules.Scheduling.Domain;

public class AvailabilityRule : Entity
{
    public Guid EventTypeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}