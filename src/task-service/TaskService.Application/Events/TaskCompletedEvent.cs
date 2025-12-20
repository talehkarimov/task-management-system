namespace TaskService.Application.Events;

public sealed class TaskCompletedEvent : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid CompletedByUserId { get; }
    public DateTime OccurredOn { get; }
    public TaskCompletedEvent(Guid taskId,
        Guid completedByUserId)
    {
        TaskId = taskId;
        CompletedByUserId = completedByUserId;
        OccurredOn = DateTime.Now;
    }
}
