using AuditService.Application.Dtos;
using AuditService.Application.Interfaces;
using Common.Pagination;
using MediatR;

namespace AuditService.Application.Queries.Handlers;

public sealed class GetAuditByEntityQueryHandler(IAuditReadRepository repository) : IRequestHandler<GetAuditByEntityQuery, PagedResult<AuditRecordDto>>
{
    public async Task<PagedResult<AuditRecordDto>> Handle(GetAuditByEntityQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByEntityAsync(request.EntityId,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
