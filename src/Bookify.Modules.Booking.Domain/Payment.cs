using Bookify.Domain;

namespace Bookify.Modules.Booking.Domain;

public class Payment : Entity
{
    public Guid OrderId { get; set; }
    public string ExternalPaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal? AmountRefunded { get; set; } = decimal.Zero;
    public DateTime CreatedAt { get; set; }
    public DateTime? RefundedAt { get; set; }
}