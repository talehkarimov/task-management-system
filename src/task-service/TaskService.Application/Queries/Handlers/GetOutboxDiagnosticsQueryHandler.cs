using MediatR;
using TaskService.Application.Interfaces;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries.Handlers;

public sealed class GetOutboxDiagnosticsQueryHandler(IOutboxDiagnosticsRepository diagnosticsRepository)
    : IRequestHandler<GetOutboxDiagnosticsQuery, OutboxDiagnosticsDto>
{
    public async Task<OutboxDiagnosticsDto> Handle(
        GetOutboxDiagnosticsQuery request,
        CancellationToken cancellationToken)
    {
        return await diagnosticsRepository.GetOutboxDiagnosticsAsync(cancellationToken);
    }
}
