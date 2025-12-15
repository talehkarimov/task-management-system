namespace TaskService.Infrastructure.Persistence;

public sealed class IdempotencyRecord
{
    public Guid Id { get; set; }
    public string RequestId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
