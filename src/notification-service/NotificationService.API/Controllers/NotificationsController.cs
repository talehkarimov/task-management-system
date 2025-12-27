using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Extensions;
using NotificationService.Application.Commands;
using NotificationService.Application.Queries;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        var result = await mediator.Send(
            new GetMyNotificationsQuery(userId),
            ct);

        return Ok(result);
    }

    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> ReadAsync(
        Guid id,
        CancellationToken ct)
    {
        var userId = HttpContext.GetUserId();

        await mediator.Send(
            new MarkNotificationAsReadCommand(id, userId),
            ct);

        return NoContent();
    }
}
