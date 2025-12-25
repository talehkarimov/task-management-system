namespace TaskService.Application.Events;

public sealed class TaskCommentAddedEventV1 : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid UserId { get; }
    public string Content { get; }
    public DateTime OccurredOn { get; }
    public Guid EventId { get; } 
    public TaskCommentAddedEventV1(Guid taskId,
        string content,
        Guid userId)
    {
        TaskId = taskId;
        UserId = userId;
        Content = content;
        OccurredOn = DateTime.Now;
        EventId = Guid.NewGuid();
    }
}
