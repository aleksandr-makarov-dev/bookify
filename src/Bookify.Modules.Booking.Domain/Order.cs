using Bookify.Domain;

namespace Bookify.Modules.Booking.Domain;

public class Order : Entity
{
    public Guid EventTypeId { get; set; }
    public Guid UserId { get; set; }
    public string GuestEmail { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}