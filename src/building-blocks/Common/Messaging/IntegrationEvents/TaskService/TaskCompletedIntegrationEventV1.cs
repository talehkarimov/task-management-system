namespace Common.Messaging.IntegrationEvents.TaskService;

public sealed record TaskCompletedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    Guid CompletedByUserId
) : IIntegrationEvent;
