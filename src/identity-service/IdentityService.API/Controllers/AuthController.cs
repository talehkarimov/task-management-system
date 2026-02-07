using IdentityService.API.Models;
using IdentityService.Application.Commands;
using IdentityService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("connect")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken ct)
    {
        await _mediator.Send(new RegisterUserCommand(request), ct);
        return NoContent();
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromForm] TokenRequest request, CancellationToken ct)
    {
        if (!string.Equals(request.GrantType, "password", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new {
                error = "unsupported_grant_type",
                error_description = "Only the 'password' grant_type is supported."
            });
        }

        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new {
                error = "invalid_request",
                error_description = "Missing username or password."
            });
        }

        var result = await _mediator.Send(new LoginUserCommand(request.UserName, request.Password, request.OrganizationId), ct);
        if (result is null)
            return Unauthorized(new { error = "invalid_grant", error_description = "Invalid credentials." });

        return Ok(new {
            access_token = result.AccessToken,
            refresh_token = result.RefreshToken,
            token_type = "Bearer",
            expires_in = result.ExpiresIn
        });
    }
}
