namespace Bookify.Modules.Scheduling.Domain;

public class EventType
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TimeZone { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public Guid OwnerId { get; set; }

    public List<AvailabilityRule> AvailabilityRules { get; set; } = [];
    public List<DateOverride> DateOverrides { get; set; } = [];
    public List<BookedSlot> BookedSlots { get; set; } = [];
}