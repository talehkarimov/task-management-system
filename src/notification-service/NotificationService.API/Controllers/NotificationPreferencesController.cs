using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Extensions;
using NotificationService.Application.Commands;
using NotificationService.Application.Dtos;
using NotificationService.Application.Queries;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/notification-preferences")]
public sealed class NotificationPreferencesController(IMediator mediator)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var result = await mediator.Send(
            new GetNotificationPreferencesQuery(userId),
            ct);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(
        UpdateNotificationPreferencesRequest request,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        await mediator.Send(
            new UpdateNotificationPreferencesCommand(
                userId,
                request.EmailEnabled,
                request.InAppEnabled,
                request.Email),
            ct);

        return NoContent();
    }
}