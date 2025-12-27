using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Models;

public sealed class NotificationDelivery
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }

    public NotificationChannel Channel { get; set; }
    public DeliveryStatus Status { get; set; }

    public int AttemptCount { get; set; }
    public DateTime? LastAttemptAt { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }

    public void MarkSucceeded()
    {
        Status = DeliveryStatus.Succeeded;
        AttemptCount++;
        LastAttemptAt = DateTime.Now;
        FailureReason = null;
    }
    public void MarkFailed(string reason)
    {
        Status = DeliveryStatus.Failed;
        AttemptCount++;
        LastAttemptAt = DateTime.Now;
        FailureReason = reason;
    }
}