namespace TaskService.Application.Events.IntegrationEvents;

public sealed record TaskBlockedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    string Reason,
    Guid ChangedByUserId
) : IIntegrationEvent;
