namespace TaskService.Application.Events;

public sealed class TaskBlockedEventV1 : IDomainEvent
{
    public Guid TaskId { get; }
    public string Reason { get; }
    public Guid ChangedByUserId { get; }
    public DateTime OccurredOn { get; }
    public Guid EventId { get; }
    public TaskBlockedEventV1(Guid taskId,
        string reason,
        Guid changedByUserId)
    {
        TaskId = taskId;
        Reason = reason;
        ChangedByUserId = changedByUserId;
        OccurredOn = DateTime.Now;
        EventId = Guid.NewGuid();
    }
}
