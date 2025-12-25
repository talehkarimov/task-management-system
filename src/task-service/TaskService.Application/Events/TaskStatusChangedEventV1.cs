using TaskStatus = TaskService.Domain.Enums.TaskStatus;

namespace TaskService.Application.Events;

internal class TaskStatusChangedEventV1 : IDomainEvent
{
    public Guid TaskId { get; }
    public TaskStatus NewStatus { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }
    public Guid EventId { get; } 
    public TaskStatusChangedEventV1(Guid taskId,
        TaskStatus newStatus,
        Guid changedByUserId)
    {
        TaskId = taskId;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
        Guid EventId = Guid.NewGuid();
    }
}
