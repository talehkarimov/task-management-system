using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public sealed class NotificationDelivery
{
    public Guid Id { get; private set; }
    public Guid NotificationId { get; private set; }

    public NotificationChannel Channel { get; private set; }
    public DeliveryStatus Status { get; private set; }

    public int AttemptCount { get; private set; }
    public DateTime? LastAttemptAt { get; private set; }
    public string? FailureReason { get; private set; }
}