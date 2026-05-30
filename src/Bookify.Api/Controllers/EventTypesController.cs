using System.ComponentModel.DataAnnotations;
using Bookify.Application.Authentication;
using Bookify.Modules.Scheduling.Application.Availability;
using Bookify.Modules.Scheduling.Application.EventTypes;
using Bookify.Modules.Users.Application.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event-types")]
public class EventTypesController(IMediator mediator, IUserProvider userProvider) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEventType([FromBody] CreateEventTypeCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(value => Ok(value), BadRequest);
    }

    [HttpGet("{eventTypeId:guid}")]
    public async Task<IActionResult> GetEventType(Guid eventTypeId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEventTypeQuery() { Id = eventTypeId }, ct);

        return result.Match<IActionResult>(Ok, NotFound);
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
            TimeZone = userProvider.TimeZone
        };

        var result = await mediator.Send(query, ct);

        return Ok(result);
    }
}