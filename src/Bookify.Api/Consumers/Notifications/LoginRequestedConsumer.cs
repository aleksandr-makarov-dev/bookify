using Bookify.Modules.Notifications.Application.Email;
using Bookify.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Bookify.Api.Consumers.Notifications;

public class LoginRequestedConsumer(IMediator mediator) : IConsumer<LoginRequestedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<LoginRequestedIntegrationEvent> context)
    {
        var message = context.Message;

        var command = new NotifyLoginConfirmCommand(message.Email, message.LoginToken);

        await mediator.Send(command);
    }
}