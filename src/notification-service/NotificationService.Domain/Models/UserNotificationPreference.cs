namespace NotificationService.Domain.Models;

public sealed class UserNotificationPreference
{
    public Guid UserId { get; private set; }
    public bool InAppEnabled { get; private set; }
    public bool EmailEnabled { get; private set; }
}
