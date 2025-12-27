using TaskService.Application.Events.IntegrationEvents;

namespace TaskService.Application.Events.Mapping;

public sealed class IntegrationEventMapper : IIntegrationEventMapper
{
    public IIntegrationEvent Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            TaskCreatedEventV1 e =>
                new TaskCreatedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.ProjectId,
                    e.ReporterUserId,
                    e.AssigneeUserId,
                    e.DueDate,
                    (int)e.Priority
                ),

            TaskAssignedEventV1 e =>
                new TaskAssignedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.TaskId,
                    e.AssigneeUserId,
                    e.ChangedByUserId
                ),

            TaskStatusChangedEventV1 e =>
                new TaskStatusChangedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.TaskId,
                    (int)e.NewStatus,
                    e.ChangedByUserId
                ),

            TaskCompletedEventV1 e =>
                new TaskCompletedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.TaskId,
                    e.CompletedByUserId
                ),

            TaskBlockedEventV1 e =>
                new TaskBlockedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.TaskId,
                    e.Reason,
                    e.ChangedByUserId
                ),

            TaskCommentAddedEventV1 e =>
                new TaskCommentAddedIntegrationEventV1(
                    e.EventId,
                    e.OccurredOn,
                    e.TaskId,
                    e.UserId
                ),

            _ => throw new InvalidOperationException(
                $"No integration event mapping defined for domain event: {domainEvent.GetType().Name}")
        };
    }
}
