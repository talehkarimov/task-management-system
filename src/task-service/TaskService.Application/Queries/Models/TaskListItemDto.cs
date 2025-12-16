using TaskService.Domain.Enums;
using TaskStatus = TaskService.Domain.Enums.TaskStatus;

namespace TaskService.Application.Queries.Models;

public sealed class TaskListItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public Guid? AssigneeUserId { get; init; }
    public DateTime? DueDate { get; init; }
}
