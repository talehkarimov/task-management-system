using AuditService.Application.Dtos;
using Common.Pagination;

namespace AuditService.Application.Interfaces;

public interface IAuditReadRepository
{
    Task<PagedResult<AuditRecordDto>> GetByEntityAsync(
        Guid entityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<PagedResult<AuditRecordDto>> GetByCorrelationAsync(
        string correlationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
