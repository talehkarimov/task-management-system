namespace TaskService.Infrastructure.Outbox;

public static class OutboxPolicy
{
    public const int MaxAttempts = 10;
    public const int FailureBackoffMs = 2000;
    public const int BatchSize = 20;
}
