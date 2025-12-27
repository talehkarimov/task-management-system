using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationPreferenceProvider
{
    public Task<UserNotificationPreference> GetAsync(Guid userId, CancellationToken cancellationToken);
    Task SaveAsync(Guid userId, string email, bool emailEnabled, bool inAppEnabled);
}
