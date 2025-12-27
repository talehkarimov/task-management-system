namespace NotificationService.Contracts.IntegrationEvents;

public sealed record TaskStatusChangedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    int NewStatus,
    Guid ChangedByUserId
) : IIntegrationEvent;