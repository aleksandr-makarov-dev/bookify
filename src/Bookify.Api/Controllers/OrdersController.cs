using Bookify.Modules.Booking.Application.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(value => Ok(value), BadRequest);
    }
}