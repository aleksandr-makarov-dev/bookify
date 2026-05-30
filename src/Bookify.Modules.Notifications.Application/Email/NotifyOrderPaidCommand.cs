using Bookify.Modules.Notifications.Application.Abstract;
using ErrorOr;
using MediatR;

namespace Bookify.Modules.Notifications.Application.Email;

public sealed record NotifyOrderPaidCommand(string GuestEmail, Guid OrderId, decimal Amount, string Currency)
    : IRequest<ErrorOr<Success>>;

public class NotifyOrderPaidCommandHandler(IEmailSender emailSender)
    : IRequestHandler<NotifyOrderPaidCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        NotifyOrderPaidCommand request,
        CancellationToken cancellationToken)
    {
        var body = $"""
                    <div>
                        <p>Hello,</p>
                        <p>Your order has been successfully paid.</p
                        <p>Order ID: {request.OrderId}</p>
                        <p>Amount: {request.Amount}{request.Currency}</p>
                        <p>Thank you for your purchase.</p>
                    </div>
                    """;

        await emailSender.SendEmailAsync(
            request.GuestEmail,
            "Order Paid",
            body);

        return Result.Success;
    }
}