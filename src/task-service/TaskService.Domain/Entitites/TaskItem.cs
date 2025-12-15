using TaskService.Domain.Enums;

namespace TaskService.Domain.Entitites;

public class TaskItem
{
    public Guid Id { get; private set; } 
    public Guid ProjectId { get; private set; }

    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }

    public Enums.TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }

    public Guid ReporterUserId { get; private set; }
    public Guid? AssigneeUserId { get; private set; }

    public DateTime? DueDate {  get; private set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private TaskItem() { }

    public TaskItem(
        Guid projectId,
        string title,
        string? description,
        Guid reporterUserId,
        Guid? assigneeUserId,
        DateTime? dueDate,
        TaskPriority priority)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Title = title;
        Description = description;
        ReporterUserId = reporterUserId;
        AssigneeUserId = assigneeUserId;
        DueDate = dueDate;
        Priority = priority;
        Status = Enums.TaskStatus.ToDo;
        CreatedAt = DateTime.Now;
    }

    public void Assign(Guid assigneeUserId)
    {
        AssigneeUserId = assigneeUserId;
        UpdatedAt = DateTime.Now;
    }

    public void ChangeStatus(Enums.TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.Now;
    }

    public void Complete()
    {
        Status = Enums.TaskStatus.Done;
        UpdatedAt = DateTime.Now;
    }

    public void Block()
    {
        Status = Enums.TaskStatus.Blocked;
        UpdatedAt = DateTime.Now;
    }
}

        
