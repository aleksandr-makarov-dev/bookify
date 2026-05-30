namespace Bookify.Modules.Booking.IntegrationEvents;

public sealed record OrderPaidIntegrationEvent(
    string GuestEmail,
    Guid OrderId,
    decimal Amount,
    string Currency);