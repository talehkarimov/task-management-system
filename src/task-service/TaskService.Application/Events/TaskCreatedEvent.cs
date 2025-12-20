using TaskService.Domain.Enums;

namespace TaskService.Application.Events;

public sealed class TaskCreatedEvent : IDomainEvent
{
    public Guid ProjectId { get; }
    public string Title { get; }
    public string? Description { get; }
    public Guid ReporterUserId { get; }
    public Guid? AssigneeUserId { get; }
    public DateTime? DueDate { get; }
    public TaskPriority Priority { get; }
    public DateTime OccurredOn { get; }

    public TaskCreatedEvent(Guid projectId,
    string title,
    string? description,
    Guid reporterUserId,
    Guid? assigneeUserId,
    DateTime? dueDate,
    TaskPriority priority)
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        ReporterUserId = reporterUserId;
        AssigneeUserId = assigneeUserId;
        DueDate = dueDate;
        Priority = priority;
        OccurredOn = DateTime.Now;
    }
}
