namespace Common.Messaging.IntegrationEvents.TaskService;

public sealed record TaskCreatedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid ProjectId,
    Guid ReporterUserId,
    Guid? AssigneeUserId,
    DateTime? DueDate,
    int Priority
) : IIntegrationEvent;
