namespace Common.Messaging.IntegrationEvents.TaskService;

public sealed record TaskBlockedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    string Reason,
    Guid ChangedByUserId
) : IIntegrationEvent;
