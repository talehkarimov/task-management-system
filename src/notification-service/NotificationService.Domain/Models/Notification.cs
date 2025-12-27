using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public sealed class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Type { get;  set; } = null!;
    public string Payload { get; set; } = null!;

    public NotificationStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Read)
            return;

        Status = NotificationStatus.Read;
        ReadAt = DateTime.Now;
    }
}
