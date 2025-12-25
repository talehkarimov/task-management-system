namespace TaskService.Application.Events;

public sealed class TaskCompletedEventV1 : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid CompletedByUserId { get; }
    public DateTime OccurredOn { get; }
    public Guid EventId { get; } 
    public TaskCompletedEventV1(Guid taskId,
        Guid completedByUserId)
    {
        TaskId = taskId;
        CompletedByUserId = completedByUserId;
        OccurredOn = DateTime.Now;
        EventId = Guid.NewGuid();
    }
}
