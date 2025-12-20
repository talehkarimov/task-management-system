using TaskService.Domain.Enums;
using TaskStatus = TaskService.Domain.Enums.TaskStatus;

namespace TaskService.Application.Events;

internal class TaskStatusChangedEvent : IDomainEvent
{
    public Guid TaskId { get; }
    public TaskStatus NewStatus { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }
    public TaskStatusChangedEvent(Guid taskId,
        TaskStatus newStatus,
        Guid changedByUserId)
    {
        TaskId = taskId;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
    }
}
