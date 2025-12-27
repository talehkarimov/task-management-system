namespace TaskService.Application.Events.IntegrationEvents;

public sealed record TaskCommentAddedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    Guid CommentedByUserId
) : IIntegrationEvent;
