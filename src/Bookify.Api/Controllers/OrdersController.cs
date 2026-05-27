using Bookify.Api.Requests.Orders;
using Bookify.Modules.Booking.Application.Orders;
using Bookify.Modules.Booking.Application.Payments;
using Bookify.Modules.Users.Application.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IMediator mediator, IUserProvider userProvider) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var command = new CreateOrderCommand()
        {
            EventTypeId = request.EventTypeId,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Message = request.Message,
            UserId = userProvider.UserId,
            TimeZone = userProvider.TimeZone,
        };

        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpPost("{orderId:guid}/payments")]
    public async Task<IActionResult> CreatePayment([FromRoute] Guid orderId, CancellationToken ct)
    {
        var successfulUrl = $"{Url.Action("ConfirmPayment", "Orders",
            new { orderId }, Request.Scheme)
        }" + "?session_id={CHECKOUT_SESSION_ID}";

        var cancelUrl = $"{Url.Action("CancelPayment", "Orders",
            new { orderId }, Request.Scheme)
        }" + "?session_id={CHECKOUT_SESSION_ID}";

        var command = new CreatePaymentCommand
        {
            OrderId = orderId,
            SuccessfulUrl = successfulUrl,
            CancelUrl = cancelUrl
        };

        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpGet("{orderId:guid}/payments/confirm")]
    public async Task<IActionResult> ConfirmPayment([FromRoute] Guid orderId,
        [FromQuery(Name = "session_id")] string sessionId, CancellationToken ct)
    {
        var command = new ConfirmPaymentCommand
        {
            OrderId = orderId,
            ExternalPaymentId = sessionId
        };

        await mediator.Send(command, ct);

        return Ok();
    }

    [HttpGet("{orderId:guid}/payments/cancel")]
    public async Task<IActionResult> CancelPayment([FromRoute] Guid orderId,
        [FromQuery(Name = "session_id")] string sessionId,
        CancellationToken ct)
    {
        return Ok();
    }
}