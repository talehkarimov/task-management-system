namespace TaskService.Application.Events;

public sealed class TaskBlockedEvent : IDomainEvent
{
    public Guid TaskId { get; }
    public string Reason { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }
    public TaskBlockedEvent(Guid taskId,
        string reason,
        Guid changedByUserId)
    {
        TaskId = taskId;
        Reason = reason;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
    }
}
