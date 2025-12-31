using AuditService.Application.Dtos;
using Common.Pagination;
using MediatR;

namespace AuditService.Application.Queries;

public sealed record GetAuditByCorrelationQuery(
    string CorrelationId,
    int Page = 1,
    int PageSize = 50
) : IRequest<PagedResult<AuditRecordDto>>;
