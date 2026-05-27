using Bookify.Modules.Users.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

        var emailConfirmationLink = Url.Action("ConfirmEmail", "Users",
            new { email = result.Value.Email, token = result.Value.EmailConfirmationToken }, Request.Scheme);

        return Ok(new
        {
            EmailConfirmationLink = emailConfirmationLink
        });
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

        var loginLink = Url.Action("ConfirmLoginLink", "Users",
            new { email = result.Value.Email, token = result.Value.LoginToken }, Request.Scheme);

        return Ok(new { LoginLink = loginLink });
    }

    [HttpGet("login-link/confirm")]
    public async Task<IActionResult> ConfirmLoginLink([FromQuery] ConfirmLoginLinkCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        return result.Match<IActionResult>(Ok, Unauthorized);
    }
}