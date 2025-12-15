namespace TaskService.API.Models;
using TaskService.Domain.Enums;
public class ChangeTaskStatusRequest
{
    public TaskStatus NewStatus { get; set; }
    public Guid ChangedByUserId { get; set; }
}
