using AuditService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController(IMediator mediator) : ControllerBase
{
    [HttpGet("entity/{entityId:guid}")]
    public async Task<IActionResult> GetByEntity(
    Guid entityId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAuditByEntityQuery(entityId, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("correlation/{correlationId}")]
    public async Task<IActionResult> GetByCorrelation(
        string correlationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAuditByCorrelationQuery(correlationId, page, pageSize),
            cancellationToken);
        return Ok(result);
    }
}
