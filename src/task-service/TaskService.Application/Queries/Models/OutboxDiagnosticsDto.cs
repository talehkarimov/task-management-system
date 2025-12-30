namespace TaskService.Application.Queries.Models;

public sealed class OutboxDiagnosticsDto
{
    public int PendingCount { get; init; }
    public int FailedCount { get; init; }
    public int PoisonedCount { get; init; }

    public DateTime? OldestPendingCreatedAt { get; init; }
    public DateTime? LastSuccessfulPublishAt { get; init; }
}
