using AuditService.Application.Dtos;
using AuditService.Application.Interfaces;
using AuditService.Infrastructure.Persistence;
using Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Infrastructure.Repositories;

public sealed class AuditReadRepository : IAuditReadRepository
{
    private readonly AuditDbContext _db;

    public AuditReadRepository(AuditDbContext db)
    {
        _db = db;
    }

    public Task<PagedResult<AuditRecordDto>> GetByEntityAsync(
        Guid entityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _db.AuditRecords
            .AsNoTracking()
            .Where(x => x.EntityId == entityId)
            .OrderByDescending(x => x.OccurredOn)
            .Select(x => new AuditRecordDto
            {
                Id = x.Id,
                ServiceName = x.ServiceName,
                EventType = x.EventType,
                EntityId = x.EntityId,
                UserId = x.UserId,
                Payload = x.Payload,
                OccurredAt = x.OccurredOn,
                CorrelationId = x.CorrelationId
            });

        return query.ToPagedResultAsync(
            page,
            pageSize,
            cancellationToken);
    }

    public Task<PagedResult<AuditRecordDto>> GetByCorrelationAsync(
        string correlationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _db.AuditRecords
            .AsNoTracking()
            .Where(x => x.CorrelationId == correlationId)
            .OrderByDescending(x => x.OccurredOn)
            .Select(x => new AuditRecordDto
            {
                Id = x.Id,
                ServiceName = x.ServiceName,
                EventType = x.EventType,
                EntityId = x.EntityId,
                UserId = x.UserId,
                Payload = x.Payload,
                OccurredAt = x.OccurredOn,
                CorrelationId = x.CorrelationId
            });

        return query.ToPagedResultAsync(
            page,
            pageSize,
            cancellationToken);
    }
}
