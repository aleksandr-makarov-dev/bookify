namespace Bookify.Modules.Scheduling.Domain;

public class BookedSlot
{
    public Guid Id { get; set; }
    public Guid EventTypeId { get; set; }
    public Guid BookingId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}