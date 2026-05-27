using Bookify.Modules.Booking.Application.Abstract;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Modules.Booking.Application.Payments;

public class CreatePaymentResult
{
    public Guid PaymentId { get; set; }
}

public class CreatePaymentCommand : IRequest<ErrorOr<CreatePaymentResult>>
{
    public Guid OrderId { get; set; }
}

public class CreatePaymentCommandHandler(IBookingDataProvider bookingDataProvider)
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

        return new CreatePaymentResult()
        {
            PaymentId = Guid.NewGuid(),
        };
    }
}