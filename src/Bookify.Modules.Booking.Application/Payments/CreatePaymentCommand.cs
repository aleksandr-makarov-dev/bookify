using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Domain;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Booking.Application.Payments;

public class CreatePaymentResult
{
    public string PaymentId { get; init; }
    public string Url { get; init; }
}

public class CreatePaymentCommand : IRequest<ErrorOr<CreatePaymentResult>>
{
    public Guid OrderId { get; init; }
    public string SuccessfulUrl { get; init; }
    public string CancelUrl { get; init; }
}

public class CreatePaymentCommandHandler(IBookingDataProvider bookingDataProvider, IPaymentProvider paymentProvider)
    : IRequestHandler<CreatePaymentCommand, ErrorOr<CreatePaymentResult>>
{
    public async Task<ErrorOr<CreatePaymentResult>> Handle(CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var order = await bookingDataProvider.Orders
            .AsNoTracking()
            .Where(e => e.Id == request.OrderId)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return Error.NotFound(description: "Order not found");
        }

        var session = await paymentProvider.CreateSessionAsync(order.Id, order.Amount, order.Currency,
            request.SuccessfulUrl,
            request.CancelUrl, cancellationToken);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            Status = PaymentStatus.Pending,
            Amount = order.Amount,
            Currency = order.Currency,
            CreatedAt = DateTime.UtcNow,
            ExternalPaymentId = session.Id,
        };

        bookingDataProvider.Payments.Add(payment);
        await bookingDataProvider.SaveChangesAsync(cancellationToken);

        return new CreatePaymentResult
        {
            PaymentId = session.Id,
            Url = session.Url
        };
    }
}