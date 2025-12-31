namespace AuditService.Application.Records;

public sealed class AuditRecord
{
    public Guid Id { get; init; }

    public string ServiceName { get; init; } = default!;
    public string EventType { get; init; } = default!;
    public Guid EntityId { get; init; }

    public Guid? UserId { get; init; }

    public string Payload { get; init; } = default!;
    public DateTime OccurredOn { get; init; }

    public string CorrelationId { get; init; } = default!;
}
