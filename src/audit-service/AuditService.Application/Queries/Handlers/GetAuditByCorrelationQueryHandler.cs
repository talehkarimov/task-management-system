using AuditService.Application.Dtos;
using AuditService.Application.Interfaces;
using Common.Pagination;
using MediatR;

namespace AuditService.Application.Queries.Handlers;

internal class GetAuditByCorrelationQueryHandler(IAuditReadRepository repository) : IRequestHandler<GetAuditByCorrelationQuery, PagedResult<AuditRecordDto>>
{
    public async Task<PagedResult<AuditRecordDto>> Handle(GetAuditByCorrelationQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByCorrelationAsync(request.CorrelationId,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
