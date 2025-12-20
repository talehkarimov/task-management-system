namespace TaskService.Application.Events;

public sealed class TaskCommentAddedEvent : IDomainEvent
{
    public Guid TaskId { get; }
    public Guid UserId { get; }
    public string Content { get; }
    public DateTime OccurredOn { get; }
    public TaskCommentAddedEvent(Guid taskId,
        string content,
        Guid userId)
    {
        TaskId = taskId;
        UserId = userId;
        Content = content;
        OccurredOn = DateTime.Now;
    }
}
