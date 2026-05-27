namespace Bookify.Modules.Scheduling.Domain;

public class AvailabilityRule
{
    public Guid Id { get; set; }
    public Guid EventTypeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}