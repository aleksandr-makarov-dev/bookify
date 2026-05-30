namespace Bookify.Modules.Booking.IntegrationEvents;

public sealed record OrderCreatedIntegrationEvent(
    Guid OrderId,
    Guid EventTypeId,
    DateTime StartDateTime,
    DateTime EndDateTime);