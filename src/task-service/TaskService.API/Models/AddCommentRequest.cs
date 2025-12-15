namespace TaskService.API.Models;

public class AddCommentRequest
{
    public Guid UserId { get; set; }
    public string Content { get; set; } = null!;
}
