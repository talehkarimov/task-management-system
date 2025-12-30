using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Queries;

namespace TaskService.API.Diagnostics.Controllers;

[ApiController]
[Route("diagnostics")]
public sealed class DiagnosticsController(IMediator mediator)
    : ControllerBase
{
    [HttpGet("outbox")]
    public async Task<IActionResult> GetOutbox(
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetOutboxDiagnosticsQuery(),
            cancellationToken);

        return Ok(result);
    }
}
