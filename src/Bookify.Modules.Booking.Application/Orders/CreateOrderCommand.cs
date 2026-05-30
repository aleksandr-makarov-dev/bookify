using Bookify.Domain;
using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Domain;
using Bookify.Modules.Booking.IntegrationEvents;
using ErrorOr;
using MassTransit;
using MediatR;

namespace Bookify.Modules.Booking.Application.Orders;

public class CreateOrderResult
{
    public Guid OrderId { get; init; }
}

public sealed record CreateOrderCommand(
    Guid EventTypeId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Message,
    Guid UserId,
    string GuestEmail,
    string TimeZone) : IRequest<ErrorOr<CreateOrderResult>>;

public class CreateOderCommandHandler(IBookingDataProvider bookingDataProvider, IBus bus)
    : IRequestHandler<CreateOrderCommand, ErrorOr<CreateOrderResult>>
{
    public async Task<ErrorOr<CreateOrderResult>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);

        var utcStartDateTime = TimeZoneInfo.ConvertTimeToUtc(request.StartDateTime, timeZoneInfo);
        var utcEndDateTime = TimeZoneInfo.ConvertTimeToUtc(request.EndDateTime, timeZoneInfo);

        var orderId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            EventTypeId = request.EventTypeId,
            UserId = request.UserId,
            StartDateTime = utcStartDateTime,
            EndDateTime = utcEndDateTime,
            Amount = 15,
            Currency = Currency.EUR,
            Message = request.Message,
            GuestEmail = request.GuestEmail,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        bookingDataProvider.Orders.Add(order);
        await bookingDataProvider.SaveChangesAsync(cancellationToken);

        await bus.Publish(new OrderCreatedIntegrationEvent(order.Id,
            order.EventTypeId, order.StartDateTime, order.EndDateTime), cancellationToken);

        return new CreateOrderResult { OrderId = orderId };
    }
}