namespace TaskService.Domain.Entitites;

public class TaskComment
{
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private TaskComment() { }

    public TaskComment(Guid taskId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.Now;
    }
}