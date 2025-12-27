namespace NotificationService.Contracts.IntegrationEvents;

public sealed record TaskAssignedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    Guid AssigneeUserId,
    Guid ChangedByUserId
) : IIntegrationEvent;