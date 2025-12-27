using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken);
}
