using Bookify.Domain;

namespace Bookify.Modules.Scheduling.Domain;

public class BookedSlot : Entity
{
    public Guid EventTypeId { get; set; }
    public Guid BookingId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}