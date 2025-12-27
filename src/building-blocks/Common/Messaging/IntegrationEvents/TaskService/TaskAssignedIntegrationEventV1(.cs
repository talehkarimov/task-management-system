namespace Common.Messaging.IntegrationEvents.TaskService;

public sealed record TaskAssignedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    Guid AssigneeUserId,
    Guid ChangedByUserId
) : IIntegrationEvent;