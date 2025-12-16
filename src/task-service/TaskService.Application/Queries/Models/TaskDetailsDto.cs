using TaskService.Domain.Enums;
using TaskStatus = TaskService.Domain.Enums.TaskStatus;

namespace TaskService.Application.Queries.Models;

public sealed class TaskDetailsDto
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public Guid ReporterUserId { get; init; }
    public Guid? AssigneeUserId { get; init; }
    public DateTime? DueDate { get; init; }
}
