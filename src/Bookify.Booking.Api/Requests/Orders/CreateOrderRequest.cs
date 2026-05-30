namespace Bookify.Booking.Api.Requests.Orders;

public record CreateOrderRequest(Guid EventTypeId, DateTime StartDateTime, DateTime EndDateTime, string? Message);