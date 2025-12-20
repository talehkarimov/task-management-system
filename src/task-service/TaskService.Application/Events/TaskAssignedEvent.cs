
namespace TaskService.Application.Events;

public sealed class TaskAssignedEvent : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid AssigneeUserId { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }

    public TaskAssignedEvent(Guid taskId,
        Guid assigneeUserId,
        Guid changedByUserId)
    {
        TaskId = taskId;
        AssigneeUserId = assigneeUserId;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
    }
}
