namespace Common.Messaging.IntegrationEvents.TaskService;

public sealed record TaskCommentAddedIntegrationEventV1(
    Guid EventId,
    DateTime OccurredOn,
    Guid TaskId,
    Guid CommentedByUserId
) : IIntegrationEvent;
