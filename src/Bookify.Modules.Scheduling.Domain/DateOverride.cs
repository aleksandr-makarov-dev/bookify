using Bookify.Domain;

namespace Bookify.Modules.Scheduling.Domain;

public class DateOverride : Entity
{
    public Guid EventTypeId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsUnavailable { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}