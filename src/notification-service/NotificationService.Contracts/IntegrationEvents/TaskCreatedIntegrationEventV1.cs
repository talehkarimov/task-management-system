namespace NotificationService.Contracts.IntegrationEvents;

public sealed record TaskCreatedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid ProjectId,
    Guid ReporterUserId,
    Guid? AssigneeUserId,
    DateTime? DueDate,
    int Priority
) : IIntegrationEvent;
