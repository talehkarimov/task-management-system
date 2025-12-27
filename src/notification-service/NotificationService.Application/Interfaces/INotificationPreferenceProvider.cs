using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationPreferenceProvider
{
    public Task<UserNotificationPreference> GetAsync(Guid userId, CancellationToken cancellationToken);
}
