using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Interfaces;

public interface INotificationChannelSender
{
    NotificationChannel Channel { get; }
    Task SendAsync(Notification notification, CancellationToken cancellationToken);
}
