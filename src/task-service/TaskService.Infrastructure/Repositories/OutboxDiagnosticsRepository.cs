using BuildingBlocks.Common.Policies;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Application.Queries.Models;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public sealed class OutboxDiagnosticsRepository(TaskDbContext dbContext) : IOutboxDiagnosticsRepository
{
    public async Task<OutboxDiagnosticsDto> GetOutboxDiagnosticsAsync(CancellationToken cancellationToken)
    {
        var pending = dbContext.OutboxMessages
            .Where(x => x.ProcessedAt == null &&
                        x.AttemptCount < OutboxPolicy.MaxAttempts);

        var failed = dbContext.OutboxMessages
            .Where(x => x.AttemptCount > 0 &&
                        x.AttemptCount < OutboxPolicy.MaxAttempts);

        var poisoned = dbContext.OutboxMessages
            .Where(x => x.AttemptCount >= OutboxPolicy.MaxAttempts);

        return new OutboxDiagnosticsDto
        {
            PendingCount = await pending.CountAsync(cancellationToken),
            FailedCount = await failed.CountAsync(cancellationToken),
            PoisonedCount = await poisoned.CountAsync(cancellationToken),

            OldestPendingCreatedAt = await pending
                .OrderBy(x => x.CreatedAt)
                .Select(x => (DateTime?)x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken),

            LastSuccessfulPublishAt = await dbContext.OutboxMessages
                .Where(x => x.ProcessedAt != null)
                .OrderByDescending(x => x.ProcessedAt)
                .Select(x => x.ProcessedAt)
                .FirstOrDefaultAsync(cancellationToken)
        };
    }
}
