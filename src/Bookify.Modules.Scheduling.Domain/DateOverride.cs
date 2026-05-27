namespace Bookify.Modules.Scheduling.Domain;

public class DateOverride
{
    public Guid Id { get; set; }
    public Guid EventTypeId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsUnavailable { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}