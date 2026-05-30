using Bookify.Modules.Booking.IntegrationEvents;
using Bookify.Modules.Scheduling.Application.BookedSlots;
using MassTransit;
using MediatR;

namespace Bookify.Api.Consumers.Orders;

public class OrderCreatedConsumer(IMediator mediator) : IConsumer<OrderCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new CreateBookedSlotCommand
        {
            OrderId = message.OrderId,
            EventTypeId = message.EventTypeId,
            StartDateTime = message.StartDateTime,
            EndDateTime = message.EndDateTime
        };

        var result = await mediator.Send(command, context.CancellationToken);

        if (result.IsError)
        {
            throw new ApplicationException(result.FirstError.Description);
        }
    }
}