using AuditService.Application.Dtos;
using Common.Pagination;
using MediatR;

namespace AuditService.Application.Queries;

public sealed record GetAuditByEntityQuery(
    Guid EntityId,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<AuditRecordDto>>;
