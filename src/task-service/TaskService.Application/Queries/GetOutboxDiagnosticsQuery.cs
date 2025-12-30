using MediatR;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries;

public sealed record GetOutboxDiagnosticsQuery
    : IRequest<OutboxDiagnosticsDto>;
