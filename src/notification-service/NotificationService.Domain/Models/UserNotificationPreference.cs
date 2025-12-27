namespace NotificationService.Domain.Models;

public sealed class UserNotificationPreference
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool InAppEnabled { get; set; }
    public bool EmailEnabled { get; set; }
}
