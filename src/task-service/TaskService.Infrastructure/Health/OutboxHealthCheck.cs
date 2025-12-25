using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Health;

public sealed class OutboxHealthCheck(TaskDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var pending = await dbContext.OutboxMessages
            .AsNoTracking()
            .CountAsync(x => x.ProcessedAt == null, cancellationToken);

        var stuck = await dbContext.OutboxMessages
            .AsNoTracking()
            .CountAsync(x => x.ProcessedAt == null && x.AttemptCount >= 10, cancellationToken);

        var data = new Dictionary<string, object>
        {
            ["outbox_pending"] = pending,
            ["outbox_stuck"] = stuck
        };

        if (stuck > 0)
        {
            return HealthCheckResult.Degraded(
                "Poisoned outbox messages detected",
                data: data);
        }

        return HealthCheckResult.Healthy("Outbox is healthy.", data: data);
    }
}
