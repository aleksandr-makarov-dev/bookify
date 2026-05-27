using System.ComponentModel.DataAnnotations;
using Bookify.Modules.Scheduling.Application.Availability;
using Bookify.Modules.Scheduling.Application.EventTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event-types")]
public class EventTypesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(value => Ok(value), BadRequest);
    }

    [HttpGet("{eventTypeId:guid}/availability")]
    public async Task<IActionResult> GetAvailability([FromRoute] Guid eventTypeId,
        [FromQuery, Required] DateOnly startDate, [FromQuery, Required] DateOnly endDate, CancellationToken ct)
    {
        var query = new GetAvailabilityQuery
        {
            EventTypeId = eventTypeId,
            StartDate = startDate,
            EndDate = endDate,
            TimeZone = "Europe/Helsinki"
        };

        var result = await mediator.Send(query, ct);
        
        return Ok(result);
    }
}