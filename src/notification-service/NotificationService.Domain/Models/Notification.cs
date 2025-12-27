using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public sealed class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public NotificationType Type { get;  set; }
    public string Payload { get; set; } = null!;

    public NotificationStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
