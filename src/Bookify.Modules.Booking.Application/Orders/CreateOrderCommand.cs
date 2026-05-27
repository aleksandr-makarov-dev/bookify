using Bookify.Common;
using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Domain;
using ErrorOr;
using MediatR;

namespace Bookify.Modules.Booking.Application.Orders;

public class CreateOrderResult
{
    public Guid OrderId { get; init; }
}

public class CreateOrderCommand : IRequest<ErrorOr<CreateOrderResult>>
{
    public Guid EventTypeId { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public string? Message { get; set; }
    public Guid UserId { get; init; }
}

public class CreateOderCommandHandler(IBookingDataProvider bookingDataProvider)
    : IRequestHandler<CreateOrderCommand, ErrorOr<CreateOrderResult>>
{
    public async Task<ErrorOr<CreateOrderResult>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            EventTypeId = request.EventTypeId,
            UserId = request.UserId,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Amount = 0,
            Currency = Currency.EUR,
            Message = request.Message,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        bookingDataProvider.Orders.Add(order);
        await bookingDataProvider.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult { OrderId = orderId };
    }
}