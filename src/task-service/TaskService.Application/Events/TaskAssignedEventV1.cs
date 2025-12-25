
namespace TaskService.Application.Events;

public sealed class TaskAssignedEventV1 : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid AssigneeUserId { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }
    public Guid EventId { get; }

    public TaskAssignedEventV1(Guid taskId,
        Guid assigneeUserId,
        Guid changedByUserId)
    {
        TaskId = taskId;
        AssigneeUserId = assigneeUserId;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
        EventId = Guid.NewGuid();
    }
}
