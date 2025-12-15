namespace TaskService.API.Models;

public class AssignTaskRequest
{
    public Guid AssigneeUserId { get; set; }
    public Guid ChangedByUserId { get; set; }
}
