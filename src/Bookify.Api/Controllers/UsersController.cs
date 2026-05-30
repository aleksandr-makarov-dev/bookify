using Bookify.Modules.Users.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(
    IMediator mediator)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(_ => Ok("Email confirmed"), Unauthorized);
    }

    [HttpPost("login-link")]
    public async Task<IActionResult> RequestLoginLink([FromBody] RequestLoginLinkCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (result.IsError)
        {
            return Unauthorized(result.Errors);
        }

        return Ok();
    }

    [HttpGet("login-link/confirm")]
    public async Task<IActionResult> ConfirmLoginLink([FromQuery] ConfirmLoginLinkCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(Ok, Unauthorized);
    }
}