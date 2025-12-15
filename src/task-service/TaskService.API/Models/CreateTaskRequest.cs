using TaskService.Domain.Enums;

namespace TaskService.API.Models;

public class CreateTaskRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ReporterUserId { get; set; }
    public Guid? AssigneeUserId { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
}
