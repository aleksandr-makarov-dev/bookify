using Bookify.Modules.Notifications.Application.Email;
using Bookify.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Bookify.Api.Consumers.Notifications;

public class UserRegisteredConsumer(IMediator mediator) : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var message = context.Message;

        await mediator.Send(new NotifyEmailConfirmCommand(message.Email, message.EmailConfirmationToken));
    }
}