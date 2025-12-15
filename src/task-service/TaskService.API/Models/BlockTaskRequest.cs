namespace TaskService.API.Models;

public class BlockTaskRequest
{
    public string Reason { get; set; } = null!;
    public Guid ChangedByUserId { get; set; }
}
