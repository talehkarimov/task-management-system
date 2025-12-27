using NotificationService.Application.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationDispatcherService
{
    Task DispatchAsync(NotificationIntent intent, CancellationToken cancellationToken);
}
