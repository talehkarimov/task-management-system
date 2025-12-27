using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationDeliveryRepository
{
    Task AddAsync(NotificationDelivery delivery, CancellationToken cancellationToken);
    Task UpdateAsync(NotificationDelivery delivery, CancellationToken cancellationToken);
}
