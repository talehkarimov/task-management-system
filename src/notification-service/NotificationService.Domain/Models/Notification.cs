using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    public string Type { get; private set; }
    public string Payload { get; private set; }

    public NotificationStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Read)
            return;

        Status = NotificationStatus.Read;
        ReadAt = DateTime.Now;
    }
}
