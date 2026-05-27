namespace Bookify.Modules.Booking.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid EventTypeId { get; set; }
    public Guid GuestId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    private List<Payment> Payments { get; set; } = [];
}

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string ExternalPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}