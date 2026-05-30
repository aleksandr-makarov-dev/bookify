using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Domain;
using Bookify.Modules.Booking.IntegrationEvents;
using ErrorOr;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Booking.Application.Payments;

public class ConfirmPaymentCommand : IRequest<ErrorOr<Success>>
{
    public Guid OrderId { get; init; }
    public string ExternalPaymentId { get; init; }
}

public class ConfirmPaymentCommandHandler(
    IBookingDataProvider bookingDataProvider,
    IPaymentProvider paymentProvider,
    IBus bus)
    : IRequestHandler<ConfirmPaymentCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await bookingDataProvider.Orders
            .FirstOrDefaultAsync(e => e.Id == request.OrderId, cancellationToken);

        var payment = await bookingDataProvider.Payments
            .FirstOrDefaultAsync(e => e.OrderId == request.OrderId && e.ExternalPaymentId == request.ExternalPaymentId,
                cancellationToken);

        if (order is null || payment is null)
        {
            return Error.Failure(description: "Invalid orderId or externalPaymentId");
        }

        var session = await paymentProvider.GetSessionStatusAsync(request.ExternalPaymentId, cancellationToken);

        if (session is null)
        {
            return Error.Failure(description: "Invalid orderId or externalPaymentId");
        }

        if (session.PaymentStatus != "paid")
        {
            return Error.Failure(description: "Payment is not completed");
        }

        order.Status = OrderStatus.Confirmed;
        payment.Status = PaymentStatus.Confirmed;

        await bookingDataProvider.SaveChangesAsync(cancellationToken);

        await bus.Publish(
            new OrderPaidIntegrationEvent(order.GuestEmail, order.Id, payment.Amount, payment.Currency),
            cancellationToken);


        return Result.Success;
    }
}