using Bookify.Modules.Booking.IntegrationEvents;
using Bookify.Modules.Notifications.Application.Email;
using MassTransit;
using MediatR;

namespace Bookify.Api.Consumers.Notifications;

public class OrderPaidConsumer(IMediator mediator) : IConsumer<OrderPaidIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPaidIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new NotifyOrderPaidCommand(message.GuestEmail, message.OrderId, message.Amount, message.Currency);

        await mediator.Send(command, context.CancellationToken);
    }
}