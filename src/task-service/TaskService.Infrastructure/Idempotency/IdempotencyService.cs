using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Idempotency;

public sealed class IdempotencyService(TaskDbContext dbContext) : IIdempotencyService
{
    public async Task<bool> IsRequestProcessedAsync(
        string requestId,
        CancellationToken cancellationToken)
    {
        return await dbContext.IdempotencyRecords
            .AnyAsync(x => x.RequestId == requestId, cancellationToken);
    }

    public async Task MarkRequestAsProcessedAsync(
        string requestId,
        CancellationToken cancellationToken)
    {
        dbContext.IdempotencyRecords.Add(new IdempotencyRecord
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
