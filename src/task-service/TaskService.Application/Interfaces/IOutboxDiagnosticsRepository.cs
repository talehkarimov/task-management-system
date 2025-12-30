using TaskService.Application.Queries.Models;

namespace TaskService.Application.Interfaces;

public  interface IOutboxDiagnosticsRepository
{
    Task<OutboxDiagnosticsDto> GetOutboxDiagnosticsAsync(
        CancellationToken cancellationToken);
}
